using System.Reflection;
using Filer.TelegramBot.Presentation.UserStates;
using Microsoft.EntityFrameworkCore;

namespace Filer.TelegramBot.Presentation.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserState> UserStates { get; set; }

    public DbSet<UserCallback> UserCallbacks { get; set; }
    
    public DbSet<UserWorkflow> UserWorkflows { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseSnakeCaseNamingConvention();
    }
}