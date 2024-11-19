using System.Reflection;
using Filer.Common.Infrastructure.Persistence.Extensions;
using Filer.Common.Presentation.Endpoints;
using Filer.Common.Presentation.OpenTelemetry;
using Filer.Storage;
using Filer.Storage.Shared.FileStorage;
using Filer.Storage.Shared.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterOpenTelemetry(
    Assembly.GetExecutingAssembly().GetName().Name!,
    builder.Configuration["Jaeger:Endpoint"]!);

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

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();