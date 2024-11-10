using Filer.TelegramBot.Presentation.UserStates.Workflows;

namespace Filer.TelegramBot.Presentation.UserStates;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterUserStates(this IServiceCollection services)
    {
        services.AddTransient<IWorkflow, CreateDirectoryWorkflow>();
        
        return services;
    }
}