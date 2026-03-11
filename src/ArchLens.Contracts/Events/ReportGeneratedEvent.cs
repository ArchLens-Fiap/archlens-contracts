namespace ArchLens.Contracts.Events;

public record ReportGeneratedEvent(
    Guid ReportId,
    Guid AnalysisId,
    Guid DiagramId,
    DateTime Timestamp);
