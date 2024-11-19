using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Filer.Common.Presentation.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterOpenTelemetry(
        this IServiceCollection services,
        string serviceName,
        string endpoint)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql();

                tracing.AddOtlpExporter(configure =>
                {
                    configure.Endpoint = new Uri(endpoint);
                });
            });

        return services;
    }
}