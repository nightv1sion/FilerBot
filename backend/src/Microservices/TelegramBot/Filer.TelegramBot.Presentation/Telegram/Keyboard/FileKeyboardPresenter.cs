using Filer.TelegramBot.Presentation.UserStates;
using Filer.TelegramBot.Presentation.UserStates.Callbacks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Telegram.Keyboard;

public sealed class FileKeyboardPresenter(CallbackSerializer callbackSerializer)
{
    public Result OpenFile(
        FileInfo file,
        string userId)
    {
        var keyboard = new InlineKeyboardMarkup();
        
        var backDirectoryCallback = AddBackButton(
            keyboard,
            userId,
            file.ParentDirectoryId);
        
        var downloadFileCallback = AddDownloadFileButton(
            keyboard,
            userId,
            file.Id);
        
        return new Result(keyboard, [
            backDirectoryCallback,
            downloadFileCallback]);
    }
    
    private UserCallback AddDownloadFileButton(
        InlineKeyboardMarkup keyboard,
        string userId,
        Guid fileId)
    {
        var uploadFileCallback = UserCallback.Create(
            Guid.NewGuid(), 
            userId, 
            callbackSerializer.Serialize(DownloadFileCallback.Create(fileId)));
        keyboard.AddDownloadFileButton(uploadFileCallback.Id.ToString());
        return uploadFileCallback;
    }
    
    private UserCallback AddBackButton(
        InlineKeyboardMarkup keyboard,
        string userId,
        Guid? directoryId)
    {
        var backButton = UserCallback.Create(
            Guid.NewGuid(), 
            userId, 
            callbackSerializer.Serialize(OpenDirectoryCallback.Create(directoryId)));
        keyboard.AddBackFolderButton(backButton.Id.ToString());
        return backButton;
    }
    
    public sealed record FileInfo(
        Guid Id,
        string Name,
        string Path,
        Guid? ParentDirectoryId);
    
    public sealed record Result(
        InlineKeyboardMarkup Keyboard,
        IReadOnlyCollection<UserCallback> UserCallbacks);
}