using Microsoft.EntityFrameworkCore;
using SquadronApi.Data;
using SquadronApi.Services;
using SquadronApi.Services.Contracts;

namespace SquadronApi.Registers;

public static partial class Register
{
    public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Dotnet build in services
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddDbContext<DataContext>(options =>
            options.UseSqlite(config.GetConnectionString("SqlLight")));

        // Dependency injection of custom Services
        services.AddScoped<IFileService, FileService>();

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}