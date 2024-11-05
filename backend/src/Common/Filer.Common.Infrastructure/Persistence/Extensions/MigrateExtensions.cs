using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Filer.Common.Infrastructure.Persistence.Extensions;

public static class MigrateExtensions
{
    public static async Task MigrateDatabase<TDbContext>(this IServiceProvider services)
        where TDbContext : DbContext
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.MigrateAsync();
    }
}