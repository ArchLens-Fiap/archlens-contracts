namespace ArchLens.Contracts.Events;

public record ProcessingStartedEvent
{
    public Guid AnalysisId { get; init; }
    public Guid DiagramId { get; init; }
    public string StoragePath { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
