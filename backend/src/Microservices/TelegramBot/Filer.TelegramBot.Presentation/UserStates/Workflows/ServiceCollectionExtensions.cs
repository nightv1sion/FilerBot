namespace Filer.TelegramBot.Presentation.UserStates.Workflows;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterWorkflows(this IServiceCollection services)
    {
        services.AddScoped<WorkflowSerializer>();
        
        return services;
    }
}