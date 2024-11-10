using Telegram.Bot.Types;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public interface ICallback
{
    Task Handle(
        IServiceProvider serviceProvider,
        CallbackQuery callbackQuery,
        CancellationToken cancellationToken);
}