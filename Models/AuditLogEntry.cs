namespace AuditLogs.Api.Models;

public class AuditLogEntry
{
    public long Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string? EntityId { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime OccuredOnUtc { get; set; } =  DateTime.UtcNow;
    public string? MetadataJson { get; set; } = string.Empty;
}