namespace ArchLens.Contracts.Events;

public record DiagramUploadedEvent(
    Guid DiagramId,
    string FileName,
    string FileHash,
    string StoragePath,
    string? UserId,
    DateTime Timestamp);
