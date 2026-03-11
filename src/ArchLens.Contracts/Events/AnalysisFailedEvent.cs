namespace ArchLens.Contracts.Events;

public record AnalysisFailedEvent(
    Guid AnalysisId,
    Guid DiagramId,
    string ErrorMessage,
    IReadOnlyList<string> FailedProviders,
    DateTime Timestamp);
