namespace Filer.Common.Infrastructure.Events;

public sealed record RabbitMqSettings(
    string Host,
    string Prefix,
    string Username = "guest",
    string Password = "guest");
