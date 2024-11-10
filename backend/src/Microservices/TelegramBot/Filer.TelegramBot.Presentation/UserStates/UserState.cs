namespace Filer.TelegramBot.Presentation.UserStates;

public sealed class UserState
{
    public required Guid Id { get; init; }
    
    public required string UserId { get; init; }

    public Guid? CurrentWorkflowId { get; set; }

    public UserWorkflow? CurrentWorkflow { get; set; }
    
    public static UserState Create(
        Guid id,
        string userId)
    {
        return new UserState
        {
            Id = id,
            UserId = userId,
        };
    }
}