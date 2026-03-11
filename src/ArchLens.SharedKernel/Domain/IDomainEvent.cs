namespace ArchLens.SharedKernel.Domain;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}
