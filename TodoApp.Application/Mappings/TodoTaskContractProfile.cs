using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApp.Application.Mappings
{
    public class TodoTaskContractProfile : Profile
    {
        public TodoTaskContractProfile()
        {
            CreateMap<TodoTaskEntity, TodoTaskListItemDto>()
    .ForCtorParam(
        nameof(TodoTaskListItemDto.Status),
        opt => opt.MapFrom(
            s => TodoTaskStatusContractMapper.ToContract(s.Status)
        ))
    .ForCtorParam(
        nameof(TodoTaskListItemDto.AllowedStatusTransitions),
        opt => opt.MapFrom(
            s => s.GetAllowedStatusTransitions()
                  .Select(TodoTaskStatusContractMapper.ToContract)
                  .ToList()
        ));
        }
    }
}
