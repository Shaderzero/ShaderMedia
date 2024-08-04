using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common.Dictionaries;
using Common.Models;
using Common.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;

namespace Common.Services;

public class AuthStateProvider(IStorage storage, ILogger<AuthStateProvider> logger) : AuthenticationStateProvider, IAuthService, IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private AuthenticationState? _state;
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_state is null)
        {
            var token = await storage.GetStringAsync(StorageItems.Token, _cts.Token);
            if (string.IsNullOrEmpty(token))
            {
                _state = new(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                SetStateFromGlobalAuth(token);
            }
        }
        return _state!;
    }
    
    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await storage.ClearAsync(cancellationToken);
        ResetStateToAnonymous();
    }

    public async Task SetLostAuthAsync(string token, CancellationToken cancellationToken)
    {
        try
        {
            await storage.SetStringAsync(StorageItems.Token, token, cancellationToken);
            SetStateFromGlobalAuth(token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        catch
        {
            ResetStateToAnonymous();
            throw;
        }
    }

    private void ResetStateToAnonymous()
    {
        _state = new(new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private void SetStateFromGlobalAuth(string token)
    {
        try
        {
            var parsedToken = new JwtSecurityToken(token);
            var claims = new List<Claim>(parsedToken.Claims);
            _state = new(new ClaimsPrincipal(new ClaimsIdentity(claims, "Password")));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка обработки токена");
            _state = new(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
    }
}