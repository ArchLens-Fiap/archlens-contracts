namespace ArchLens.Contracts.Events;

public record DiagramUploadedEvent
{
    public Guid DiagramId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string FileHash { get; init; } = string.Empty;
    public string StoragePath { get; init; } = string.Empty;
    public string? UserId { get; init; }
    public DateTime Timestamp { get; init; }
}
