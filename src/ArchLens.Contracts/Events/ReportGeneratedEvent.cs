namespace ArchLens.Contracts.Events;

public record ReportGeneratedEvent
{
    public Guid ReportId { get; init; }
    public Guid AnalysisId { get; init; }
    public Guid DiagramId { get; init; }
    public DateTime Timestamp { get; init; }
}
