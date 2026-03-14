namespace ArchLens.Contracts.Events;

public record StatusChangedEvent
{
    public Guid AnalysisId { get; init; }
    public string OldStatus { get; init; } = string.Empty;
    public string NewStatus { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
