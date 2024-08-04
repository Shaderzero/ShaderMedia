using Common.Services;
using Common.Services.Books;
using Common.Services.Books.Interfaces;
using Common.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class CommonBootstrap
{
    private static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<RefreshService>();
        services.AddScoped<IApiService, ApiService>();
        
        services.AddAuthorizationCore();
        services.AddSingleton<AuthStateProvider>();
        services.AddSingleton<AuthenticationStateProvider>(serviceProvider => serviceProvider.GetRequiredService<AuthStateProvider>());
        services.AddSingleton<IAuthService>(serviceProvider => serviceProvider.GetRequiredService<AuthStateProvider>());
        services.AddScoped<IInpxService, InpxService>();
        services.AddScoped<IBookService, BookService>();

        services.AddSingleton<CallService>();

        return services;
    }

    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddCommonServices();

        return services;
    }

    public static IServiceCollection AddMauiServices(this IServiceCollection services)
    {
        services.AddCommonServices();

        return services;
    }
}
