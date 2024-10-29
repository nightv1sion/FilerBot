using Filer.Common.Infrastructure.Abstractions;
using Filer.Common.Infrastructure.Caching;
using Filer.Common.Infrastructure.Events;
using Filer.Common.Infrastructure.Persistence.Interceptors;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Filer.Common.Application.Caching;
using Filer.Common.Application.Events;
using Quartz;
using StackExchange.Redis;

namespace Filer.Common.Infrastructure;
public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
        RabbitMqSettings rabbitMqSettings,
        string databaseConnectionString,
        string redisConnectionString)
    {
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        
        services.TryAddSingleton<IEventBus, EventBus>();
        
        services.TryAddSingleton<PublishDomainEventsInterceptor>();
        
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        
        services.AddMassTransit(configure =>
        {
            foreach (Action<IRegistrationConfigurator> configureConsumers in moduleConfigureConsumers)
            {
                configureConsumers(configure);
            }

            configure.SetKebabCaseEndpointNameFormatter();
            
            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqSettings.Host), h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer));
        }
        catch
        {
            services.AddDistributedMemoryCache();
        }

        services.TryAddSingleton<ICacheService, CacheService>();

        return services;
    }
}
