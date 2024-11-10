namespace Filer.TelegramBot.Presentation.UserStates;

public sealed class UserWorkflow
{
    public required Guid Id { get; init; }
    
    public required string UserId { get; init; }

    public required string WorkflowPayload { get; set; }
    
    public static UserWorkflow Create(
        Guid id,
        string userId,
        string workflowPayload)
    {
        return new UserWorkflow
        {
            Id = id,
            UserId = userId,
            WorkflowPayload = workflowPayload,
        };
    }
}