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
        
        var downloadFileCallback = AddDownloadFileButton(
            keyboard,
            userId,
            file.Id);
        
        var removeFileCallback = AddRemoveFileButton(
            keyboard,
            userId,
            file.Id,
            file.ParentDirectoryId);
        
        keyboard.AddNewRow();
        
        var backDirectoryCallback = AddBackButton(
            keyboard,
            userId,
            file.ParentDirectoryId);
        
        return new Result(keyboard, [
            downloadFileCallback,
            removeFileCallback,
            backDirectoryCallback]);
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
    
    private UserCallback AddRemoveFileButton(
        InlineKeyboardMarkup keyboard,
        string userId,
        Guid fileId,
        Guid? parentDirectoryId)
    {
        var removeFileCallback = UserCallback.Create(
            Guid.NewGuid(), 
            userId, 
            callbackSerializer.Serialize(RemoveFileCallback.Create(fileId, parentDirectoryId)));
        keyboard.AddDeleteFileButton(removeFileCallback.Id.ToString());
        return removeFileCallback;
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