using Filer.Storage.Integration.Files.DownloadFile;
using Filer.TelegramBot.Presentation.ApiClients.Storage;
using Filer.TelegramBot.Presentation.Persistence;
using Filer.TelegramBot.Presentation.UserStates.Workflows;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using InputFile = Microsoft.AspNetCore.Components.Forms.InputFile;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class DownloadFileCallback : ICallback
{
    public required Guid FileId { get; init; }
    
    public async Task Handle(IServiceProvider serviceProvider, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var storageApi = serviceProvider.GetRequiredService<IStorageApi>();
        var bot = serviceProvider.GetRequiredService<ITelegramBotClient>();
        var userId = callbackQuery.From.Id.ToString();

        DownloadFileResponse? downloadFileResponse = await storageApi.DownloadFile(
            new DownloadFileRequest(userId, FileId),
            cancellationToken);

        if (downloadFileResponse is null)
        {
            throw new InvalidOperationException("File not found");
        }

        var fileStream = new MemoryStream(downloadFileResponse.FileBytes);

        await bot.SendDocumentAsync(
            new ChatId(callbackQuery.From.Id),
            new InputFileStream(fileStream, downloadFileResponse.FileName),
            cancellationToken: cancellationToken);
    }
    
    public static DownloadFileCallback Create(Guid fileId)
    {
        return new DownloadFileCallback
        {
            FileId = fileId
        };
    }
}