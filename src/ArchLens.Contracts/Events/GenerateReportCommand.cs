namespace ArchLens.Contracts.Events;

public record GenerateReportCommand
{
    public Guid AnalysisId { get; init; }
    public Guid DiagramId { get; init; }
    public string? UserId { get; init; }
    public string ResultJson { get; init; } = string.Empty;
    public IReadOnlyList<string> ProvidersUsed { get; init; } = [];
    public long ProcessingTimeMs { get; init; }
    public DateTime Timestamp { get; init; }
}
