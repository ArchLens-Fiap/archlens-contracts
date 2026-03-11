namespace ArchLens.Contracts.Events;

public record AnalysisCompletedEvent(
    Guid AnalysisId,
    Guid DiagramId,
    string ResultJson,
    IReadOnlyList<string> ProvidersUsed,
    long ProcessingTimeMs,
    DateTime Timestamp);
