using Application.Services;
using Domain.Ports;
using Domain.Ports.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IStateTaskRepository, StateTaskRepository>();
        services.AddScoped<ITaskPriorityRepository, TaskPriorityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRolRepository, UserRolRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
