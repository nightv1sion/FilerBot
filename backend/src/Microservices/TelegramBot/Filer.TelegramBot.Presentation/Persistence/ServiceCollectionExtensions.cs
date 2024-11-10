using Microsoft.EntityFrameworkCore;

namespace Filer.TelegramBot.Presentation.Persistence;

public static class ServiceCollectionExtensions
{
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