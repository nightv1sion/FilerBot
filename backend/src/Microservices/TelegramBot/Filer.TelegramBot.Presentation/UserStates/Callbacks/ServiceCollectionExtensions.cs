namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCallbacks(this IServiceCollection services)
    {
        services.AddScoped<CallbackSerializer>();
        
        return services;
    }
}