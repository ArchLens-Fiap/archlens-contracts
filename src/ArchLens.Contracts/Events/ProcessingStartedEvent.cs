namespace ArchLens.Contracts.Events;

public record ProcessingStartedEvent(
    Guid AnalysisId,
    Guid DiagramId,
    string StoragePath,
    DateTime Timestamp);
