namespace ArchLens.Contracts.Events;

public record UserAccountDeletedEvent(
    Guid UserId,
    DateTime Timestamp);
