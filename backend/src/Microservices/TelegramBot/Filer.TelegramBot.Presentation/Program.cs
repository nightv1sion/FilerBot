using System.Reflection;
using Filer.Common.Infrastructure.Persistence.Extensions;
using Filer.Common.Presentation.Endpoints;
using Filer.Common.Presentation.Middlewares;
using Filer.Common.Presentation.OpenTelemetry;
using Filer.TelegramBot.Presentation.ApiClients;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.Telegram;
using Filer.TelegramBot.Presentation.UserStates.Callbacks;
using Filer.TelegramBot.Presentation.UserStates.Workflows;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

builder.Services.RegisterOpenTelemetry(
    Assembly.GetExecutingAssembly().GetName().Name!,
    builder.Configuration["Seq:Endpoint"]!);

builder.Services.RegisterRefitClients(builder.Configuration);
builder.Services.RegisterTelegramIntegration(builder.Configuration);
builder.Services.RegisterPersistence(builder.Configuration);
builder.Services.RegisterCallbacks();
builder.Services.RegisterWorkflows();

WebApplication app = builder.Build();

await app.Services.MigrateDatabase<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapEndpoints();

app.Run();