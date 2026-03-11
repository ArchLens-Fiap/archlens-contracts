namespace ArchLens.Contracts.Events;

public record ReportFailedEvent(
    Guid AnalysisId,
    Guid DiagramId,
    string ErrorMessage,
    DateTime Timestamp);
