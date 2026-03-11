namespace ArchLens.Contracts.Events;

public record StatusChangedEvent(
    Guid AnalysisId,
    string OldStatus,
    string NewStatus,
    DateTime Timestamp);
