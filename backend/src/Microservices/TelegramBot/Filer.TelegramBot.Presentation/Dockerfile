FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservices/TelegramBot/Filer.TelegramBot.Presentation/Filer.TelegramBot.Presentation.csproj", "Microservices/TelegramBot/Filer.TelegramBot.Presentation/"]
COPY ["Common/Filer.Common.Infrastructure/Filer.Common.Infrastructure.csproj", "Common/Filer.Common.Infrastructure/"]
COPY ["Common/Filer.Common.Application/Filer.Common.Application.csproj", "Common/Filer.Common.Application/"]
COPY ["Common/Filer.Common.Domain/Filer.Common.Domain.csproj", "Common/Filer.Common.Domain/"]
COPY ["Common/Filer.Common.Presentation/Filer.Common.Presentation.csproj", "Common/Filer.Common.Presentation/"]
COPY ["Microservices/Storage/Filer.Storage.Integration/Filer.Storage.Integration.csproj", "Microservices/Storage/Filer.Storage.Integration/"]
RUN dotnet restore "Microservices/TelegramBot/Filer.TelegramBot.Presentation/Filer.TelegramBot.Presentation.csproj"
COPY . .
WORKDIR "/src/Microservices/TelegramBot/Filer.TelegramBot.Presentation"
RUN dotnet build "Filer.TelegramBot.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Filer.TelegramBot.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filer.TelegramBot.Presentation.dll"]
