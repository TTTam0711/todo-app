using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.UseCases.Lists;

namespace TodoApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Lists CRUD
            services.AddScoped<CreateTodoListUseCase>();
            services.AddScoped<GetTodoListsUseCase>();
            services.AddScoped<GetTodoListByIdUseCase>();
            services.AddScoped<UpdateTodoListUseCase>();
            services.AddScoped<SetArchiveTodoListUseCase>();
            services.AddScoped<DeleteTodoListUseCase>();

            // Tasks/Subtasks/Tags: đăng ký sau
            // services.AddScoped<CreateTodoTaskUseCase>(); ...

            return services;
        }
    }
}
