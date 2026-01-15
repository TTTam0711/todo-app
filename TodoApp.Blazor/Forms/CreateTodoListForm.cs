using System.ComponentModel.DataAnnotations;

namespace TodoApp.Blazor.Forms
{
    public class CreateTodoListForm
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Color { get; set; }
    }
}
