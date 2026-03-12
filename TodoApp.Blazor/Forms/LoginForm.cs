using System.ComponentModel.DataAnnotations;

namespace TodoApp.Blazor.Forms
{
    public sealed class LoginForm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(6)]
        public string Password { get; set; } = "";
    }
}
