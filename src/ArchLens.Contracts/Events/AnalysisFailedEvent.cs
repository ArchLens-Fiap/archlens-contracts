namespace ArchLens.Contracts.Events;

public record AnalysisFailedEvent
{
    public Guid AnalysisId { get; init; }
    public Guid DiagramId { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public IReadOnlyList<string> FailedProviders { get; init; } = [];
    public DateTime Timestamp { get; init; }
}
