using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Factories;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Mappings
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            // TodoList
            CreateMap<TodoList, TodoListEntity>()
                .ConstructUsing(src =>
                    TodoListFactory.RestoreFromDb(
                        src.ListId,
                        src.OwnerUserId,
                        src.Name,
                        src.Color,
                        src.IsArchived,
                        src.IsDeleted,
                        src.DeletedAt,
                        src.CreatedAt,
                        src.UpdatedAt,
                        src.RowVersion
                    ))
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<TodoListEntity, TodoList>()
                .ForMember(d => d.OwnerUser, opt => opt.Ignore())
                .ForMember(d => d.TodoTasks, opt => opt.Ignore());

            // TodoTask (Status string <-> enum)
            CreateMap<TodoTask, TodoTaskEntity>()
                .ConstructUsing(src =>
                    TodoTaskFactory.RestoreFromDb(
                        src.TaskId,
                        src.ListId,
                        src.Title,
                        MapStatusToEnum(src.Status),
                        src.IsDeleted,
                        src.DeletedAt,
                        src.OrderIndex,
                        src.Priority,
                        src.Description,
                        src.CreatedByUserId,
                        src.UpdatedByUserId,
                        src.CreatedAt,
                        src.UpdatedAt,
                        src.DueAt,
                        src.CompletedAt,
                        src.RowVersion
                    ))
                .ForAllMembers(opt => opt.Ignore());


            CreateMap<TodoTaskEntity, TodoTask>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => MapEnumToStatus(s.Status)))
                .ForMember(d => d.Attachments, opt => opt.Ignore())
                .ForMember(d => d.Comments, opt => opt.Ignore())
                .ForMember(d => d.Reminders, opt => opt.Ignore())
                .ForMember(d => d.Subtasks, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore())
                .ForMember(d => d.List, opt => opt.Ignore())
                .ForMember(d => d.CreatedByUser, opt => opt.Ignore())
                .ForMember(d => d.UpdatedByUser, opt => opt.Ignore())
                .ForMember(d => d.DueAt, opt => opt.MapFrom(s => s.DueAt))
                .ForMember(d => d.CompletedAt, opt => opt.MapFrom(s => s.CompletedAt))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.Priority, opt => opt.MapFrom(s => s.Priority));



            // ✅ Subtask (ignore navigation Task)
            CreateMap<Subtask, SubtaskEntity>()
                .ForSourceMember(s => s.Task, opt => opt.DoNotValidate());

            CreateMap<SubtaskEntity, Subtask>()
                .ForMember(d => d.Task, opt => opt.Ignore());

            // ✅ Tag (ignore navigation OwnerUser & Tasks)
            CreateMap<Tag, TagEntity>()
                .ForSourceMember(s => s.OwnerUser, opt => opt.DoNotValidate())
                .ForSourceMember(s => s.Tasks, opt => opt.DoNotValidate());

            CreateMap<TagEntity, Tag>()
                .ForMember(d => d.OwnerUser, opt => opt.Ignore())
                .ForMember(d => d.Tasks, opt => opt.Ignore());
        }

        private static TodoTaskStatus MapStatusToEnum(string status)
            => status.Trim().ToLowerInvariant() switch
            {
                "todo" => TodoTaskStatus.Todo,
                "inprogress" => TodoTaskStatus.InProgress,
                "in_progress" => TodoTaskStatus.InProgress,
                "done" => TodoTaskStatus.Done,
                "blocked" => TodoTaskStatus.Blocked,
                "cancelled" => TodoTaskStatus.Cancelled,
                _ => TodoTaskStatus.Todo
            };

        private static string MapEnumToStatus(TodoTaskStatus status)
            => status switch
            {
                TodoTaskStatus.Todo => "Todo",
                TodoTaskStatus.InProgress => "InProgress",
                TodoTaskStatus.Done => "Done",
                TodoTaskStatus.Blocked => "Blocked",
                TodoTaskStatus.Cancelled => "Cancelled",
                _ => "Todo"
            };
    }
}
