using Filer.TelegramBot.Presentation.UserStates;
using Filer.TelegramBot.Presentation.UserStates.Callbacks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Telegram.Keyboard;

internal sealed class DirectoryKeyboardPresenter(
    CallbackSerializer callbackSerializer)
{
    public Result OpenRootDirectory(
        IReadOnlyCollection<DirectoryButton> buttons,
        IReadOnlyCollection<FileButton> files,
        string userId)
    {
        var keyboard = new InlineKeyboardMarkup();

        var openCallbacks = AddOpenDirectoriesButtons(
            keyboard,
            buttons,
            userId);
        
        keyboard.AddNewRow();
        
        var openFilesCallbacks = AddOpenFilesButtons(
            keyboard,
            userId,
            files);
        
        keyboard.AddNewRow();

        var createCallback = AddCreateDirectoryButton(
            keyboard,
            userId,
            null);
        
        var uploadFileCallback = AddUploadFileButton(
            keyboard,
            userId,
            null);

        return new Result(keyboard, [
            ..openCallbacks,
            ..openFilesCallbacks,
            createCallback,
            uploadFileCallback]);
    }

    public Result OpenDirectory(
        IReadOnlyCollection<DirectoryButton> buttons,
        IReadOnlyCollection<FileButton> files,
        string userId,
        Guid directoryId,
        Guid? parentDirectoryId)
    {
        var keyboard = new InlineKeyboardMarkup();

        var openDirectoriesCallbacks = AddOpenDirectoriesButtons(
            keyboard,
            buttons,
            userId);
        
        keyboard.AddNewRow();
        
        var openFilesCallbacks = AddOpenFilesButtons(
            keyboard,
            userId,
            files);
        
        keyboard.AddNewRow();

        var createCallback = AddCreateDirectoryButton(
            keyboard,
            userId,
            directoryId);
        
        var removeDirectoryCallback = AddRemoveDirectoryButton(
            keyboard,
            userId,
            directoryId);
        
        keyboard.AddNewRow();
        
        var uploadFileCallback = AddUploadFileButton(
            keyboard,
            userId,
            directoryId);
        
        var backDirectoryCallback = AddBackButton(
            keyboard,
            userId,
            parentDirectoryId);
        
        return new Result(
            keyboard, [
                ..openDirectoriesCallbacks,
                ..openFilesCallbacks,
                createCallback,
                removeDirectoryCallback,
                uploadFileCallback,
                backDirectoryCallback]);
    }

    private IReadOnlyCollection<UserCallback> AddOpenDirectoriesButtons(
        InlineKeyboardMarkup keyboard,
        IReadOnlyCollection<DirectoryButton> buttons,
        string userId)
    {
        List<UserCallback> userCallbacks = new(buttons.Count);

        foreach (var button in buttons)
        {
            var openDirectoryCallback = UserCallback.Create(
                Guid.NewGuid(), 
                userId, 
                callbackSerializer.Serialize(OpenDirectoryCallback.Create(button.Id)));
            userCallbacks.Add(openDirectoryCallback);
            
            keyboard.AddNewRow();
            keyboard.AddOpenFolderButton(button.Name, openDirectoryCallback.Id.ToString());
        }

        return userCallbacks;
    }

    private UserCallback AddCreateDirectoryButton(
        InlineKeyboardMarkup keyboard,
        string userId,
        Guid? directoryId)
    {
        var createDirectoryCallback = UserCallback.Create(
            Guid.NewGuid(), 
            userId, 
            callbackSerializer.Serialize(CreateDirectoryCallback.Create(directoryId)));
        keyboard.AddCreateFolderButton(createDirectoryCallback.Id.ToString());
        return createDirectoryCallback;
    }
    
    private UserCallback AddRemoveDirectoryButton(
        InlineKeyboardMarkup keyboard,
        string userId,
        Guid directoryId)
    {
        var removeDirectoryCallback = UserCallback.Create(
            Guid.NewGuid(), 
            userId, 
            callbackSerializer.Serialize(RemoveDirectoryCallback.Create(directoryId)));
        keyboard.AddDeleteFolderButton(removeDirectoryCallback.Id.ToString());
        return removeDirectoryCallback;
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

    private UserCallback AddUploadFileButton(
        InlineKeyboardMarkup keyboard,
        string userId,
        Guid? directoryId)
    {
        var uploadFileCallback = UserCallback.Create(
            Guid.NewGuid(), 
            userId, 
            callbackSerializer.Serialize(UploadFileCallback.Create(directoryId)));
        keyboard.AddUploadFileButton(uploadFileCallback.Id.ToString());
        return uploadFileCallback;
    }
    
    private IReadOnlyCollection<UserCallback> AddOpenFilesButtons(
        InlineKeyboardMarkup keyboard,
        string userId,
        IReadOnlyCollection<FileButton> buttons)
    {
        List<UserCallback> userCallbacks = new(buttons.Count);

        foreach (var button in buttons)
        {
            var openFileCallback = UserCallback.Create(
                Guid.NewGuid(), 
                userId, 
                callbackSerializer.Serialize(OpenFileCallback.Create(button.Id)));
            userCallbacks.Add(openFileCallback);
            
            keyboard.AddNewRow();
            keyboard.AddOpenFileButton(button.Name, openFileCallback.Id.ToString());
        }

        return userCallbacks;
    }
    
    public sealed record DirectoryButton(
        Guid Id,
        string Name);
    
    public sealed record FileButton(
        Guid Id,
        string Name);

    public sealed record Result(
        InlineKeyboardMarkup Keyboard,
        IReadOnlyCollection<UserCallback> UserCallbacks);
}