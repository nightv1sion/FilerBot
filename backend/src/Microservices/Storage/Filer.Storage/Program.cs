using System.Reflection;
using Filer.Common.Infrastructure.Persistence.Extensions;
using Filer.Common.Presentation.Endpoints;
using Filer.Common.Presentation.Middlewares;
using Filer.Common.Presentation.OpenTelemetry;
using Filer.Storage;
using Filer.Storage.Shared.FileStorage;
using Filer.Storage.Shared.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, loggerConfig)
    => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.RegisterOpenTelemetry(
    Assembly.GetExecutingAssembly().GetName().Name!,
    builder.Configuration["Seq:Endpoint"]!);

builder.Services
    .RegisterApplicationServices()
    .RegisterPersistence(builder.Configuration)
    .RegisterFileStorage(builder.Configuration)
    .AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

await app.Services.MigrateDatabase<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();