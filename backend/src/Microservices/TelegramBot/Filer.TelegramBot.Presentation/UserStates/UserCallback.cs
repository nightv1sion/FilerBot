using Filer.TelegramBot.Presentation.UserStates.Callbacks;

namespace Filer.TelegramBot.Presentation.UserStates;

public sealed class UserCallback
{
    public required Guid Id { get; init; }
    
    public required string UserId { get; init; }

    public required string CallbackPayload { get; init; }
    
    public static UserCallback Create(
        Guid id,
        string userId,
        string callbackPayload)
    {
        return new UserCallback
        {
            Id = id,
            UserId = userId,
            CallbackPayload = callbackPayload,
        };
    }
}