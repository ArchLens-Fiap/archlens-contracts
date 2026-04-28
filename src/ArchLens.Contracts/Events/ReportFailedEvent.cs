namespace ArchLens.Contracts.Events;

public record ReportFailedEvent
{
    public Guid AnalysisId { get; init; }
    public Guid DiagramId { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
