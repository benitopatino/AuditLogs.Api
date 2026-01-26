namespace AuditLogs.Api.Dtos;

public record CreateAuditLogRequest(
    string Action,
    string Entity,
    string? EntityId,
    string PerformedBy,
    string? MetadataJson
    );