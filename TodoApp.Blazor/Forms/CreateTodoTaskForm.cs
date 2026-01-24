using System.ComponentModel.DataAnnotations;
using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Blazor.Forms
{
    public class CreateTodoTaskForm
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueAt { get; set; }

        public TodoTaskPriority Priority { get; set; }
            = TodoTaskPriority.None;
    }

}
