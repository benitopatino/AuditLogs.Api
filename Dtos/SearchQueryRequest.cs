namespace AuditLogs.Api.Dtos;

public record SearchQueryRequest(
    string? User,
    string? Action,
    DateTime? FromUtc,
    DateTime? ToUtc
);
