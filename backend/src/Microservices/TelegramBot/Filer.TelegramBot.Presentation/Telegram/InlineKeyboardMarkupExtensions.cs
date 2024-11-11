using Telegram.Bot.Types.ReplyMarkups;

namespace Filer.TelegramBot.Presentation.Telegram;

public static class InlineKeyboardMarkupExtensions
{
    public static InlineKeyboardMarkup AddFolderButton(
        this InlineKeyboardMarkup keyboard,
        string name,
        string callbackData)
    {
        return keyboard.AddButton($"\ud83d\udcc1 {name}", callbackData);
    }

    public static InlineKeyboardMarkup AddCreateFolderButton(
        this InlineKeyboardMarkup keyboard,
        string callbackData)
    {
        return keyboard.AddButton("\u2795 Создать папку", callbackData);
    }
    
    public static InlineKeyboardMarkup AddDeleteFolderButton(
        this InlineKeyboardMarkup keyboard,
        string callbackData)
    {
        return keyboard.AddButton("\u2796 Удалить папку", callbackData);
    }

    public static InlineKeyboardMarkup AddBackFolderButton(
        this InlineKeyboardMarkup keyboard,
        string callbackData)
    {
        return keyboard.AddButton("\u2b05 Назад", callbackData);
    }
}