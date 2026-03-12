using System.ComponentModel.DataAnnotations;

namespace TodoApp.Blazor.Forms
{
    public sealed class RegisterForm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        public string DisplayName { get; set; } = "";
    }
}
