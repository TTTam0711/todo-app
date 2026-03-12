using System.ComponentModel.DataAnnotations;

namespace TodoApp.Blazor.ViewModels.Auth
{
    public sealed class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
    }
}
