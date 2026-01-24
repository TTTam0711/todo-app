using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Mappings;
using TodoApp.Contracts.TodoTasks;
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
                        TodoTaskDbStatusMapper.ToDomain(src.Status),
                        src.IsDeleted,
                        src.DeletedAt,
                        src.OrderIndex,
                        (Domain.Entities.Enums.TodoTaskPriority)src.Priority,
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
                 .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))   // ✅ DOMAIN enum
                 .ForMember(d => d.Priority, opt => opt.MapFrom(s => s.Priority))
                 .ForMember(d => d.Attachments, opt => opt.Ignore())
                 .ForMember(d => d.Comments, opt => opt.Ignore())
                 .ForMember(d => d.Reminders, opt => opt.Ignore())
                 .ForMember(d => d.Subtasks, opt => opt.Ignore())
                 .ForMember(d => d.Tags, opt => opt.Ignore())
                 .ForMember(d => d.List, opt => opt.Ignore())
                 .ForMember(d => d.CreatedByUser, opt => opt.Ignore())
                 .ForMember(d => d.UpdatedByUser, opt => opt.Ignore());
            

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

        
    }
}
