using Filer.TelegramBot.Presentation.UserStates;
using Filer.TelegramBot.Presentation.UserStates.Callbacks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Telegram.Keyboard;

internal sealed class DirectoryKeyboardPresenter(
    CallbackSerializer callbackSerializer)
{
    public Result OpenRootDirectory(
        IReadOnlyCollection<DirectoryButton> buttons,
        string userId)
    {
        var keyboard = new InlineKeyboardMarkup();

        var openCallbacks = AddOpenButtons(
            keyboard,
            buttons,
            userId);
        
        keyboard.AddNewRow();

        var createCallback = AddCreateButton(
            keyboard,
            userId,
            null);

        return new Result(keyboard, [..openCallbacks, createCallback]);
    }

    public Result OpenDirectory(
        IReadOnlyCollection<DirectoryButton> buttons,
        string userId,
        Guid directoryId,
        Guid? parentDirectoryId)
    {
        var keyboard = new InlineKeyboardMarkup();

        var openCallbacks = AddOpenButtons(
            keyboard,
            buttons,
            userId);
        
        keyboard.AddNewRow();

        var createCallback = AddCreateButton(
            keyboard,
            userId,
            directoryId);
        
        var removeDirectoryCallback = AddRemoveButton(
            keyboard,
            userId,
            directoryId);
        
        var backDirectoryCallback = AddBackButton(
            keyboard,
            userId,
            parentDirectoryId);
        
        return new Result(
            keyboard, [
                ..openCallbacks,
                createCallback,
                removeDirectoryCallback,
                backDirectoryCallback]);
    }

    private IReadOnlyCollection<UserCallback> AddOpenButtons(
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
            keyboard.AddFolderButton(button.Name, openDirectoryCallback.Id.ToString());
        }

        return userCallbacks;
    }

    private UserCallback AddCreateButton(
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
    
    private UserCallback AddRemoveButton(
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
    
    public sealed record DirectoryButton(
        Guid Id,
        string Name);

    public sealed record Result(
        InlineKeyboardMarkup Keyboard,
        IReadOnlyCollection<UserCallback> UserCallbacks);
}