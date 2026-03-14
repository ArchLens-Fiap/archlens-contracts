namespace ArchLens.Contracts.Events;

public record UserAccountDeletedEvent
{
    public Guid UserId { get; init; }
    public DateTime Timestamp { get; init; }
}
