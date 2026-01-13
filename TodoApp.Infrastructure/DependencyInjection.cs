using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.UseCases.Lists;
using TodoApp.Infrastructure.Mappings;
using TodoApp.Infrastructure.Persistence.Context;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(TodoMappingProfile).Assembly);

            // sau này add DbContext, Repository, Redis, Auth ở đây 
            //DbContext
            services.AddDbContext<TodoAppDbContext>(options =>
                                options.UseSqlServer(
                                    configuration.GetConnectionString("Default")));
            //Repository
            services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<ISubtaskRepository, SubtaskRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            return services;

            
        }
    }
}
