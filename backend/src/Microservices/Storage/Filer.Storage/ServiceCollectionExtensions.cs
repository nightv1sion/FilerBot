using System.Reflection;
using Filer.Storage.Shared.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Filer.Storage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services
            .AddSingleton(TimeProvider.System);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection RegisterPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((_, options) =>
        {
            string connectionString = configuration.GetConnectionString("Database")!;

            options.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                builder.MigrationsHistoryTable("ef_migration_history");
            });
        });

        return services;
    }
}