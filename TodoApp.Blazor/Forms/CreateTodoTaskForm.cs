using System.ComponentModel.DataAnnotations;

namespace TodoApp.Blazor.Forms
{
    public class CreateTodoTaskForm
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueAt { get; set; }

        public byte Priority { get; set; }
    }

}
