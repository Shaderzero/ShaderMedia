using System.ComponentModel.DataAnnotations;
using Common.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Shared;
using Shared.Models;
using Shared.Models.Identity;

namespace Common.Pages.Identity;

public partial class AccountRegisterPage
{
    [Inject]
    public required IApiService ApiService { get; set; }

    private readonly CancellationTokenSource _cts = new();
    private InputModel _model = new();

    private async Task AccountRegister()
    {
        var model = new UserRegister
        {
            Email = _model.Email,
            Password = _model.Password
        };
        var result = await ApiService.PostAsync<UserRegister, ApiResponse<bool>>(ApiPaths.UserRegister, model, _cts.Token);
    }
    
    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}