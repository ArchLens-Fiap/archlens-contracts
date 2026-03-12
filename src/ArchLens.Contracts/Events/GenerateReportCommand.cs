namespace ArchLens.Contracts.Events;

public record GenerateReportCommand(
    Guid AnalysisId,
    Guid DiagramId,
    string? UserId,
    string ResultJson,
    IReadOnlyList<string> ProvidersUsed,
    long ProcessingTimeMs,
    DateTime Timestamp);
