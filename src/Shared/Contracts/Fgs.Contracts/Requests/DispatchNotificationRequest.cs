namespace Fgs.Contracts.Requests;

public sealed record DispatchNotificationRequest
{
    public string? RoutingKey { get; init; }

    public string? Payload { get; init; }

    public string? CorrelationId { get; init; }

    public string? MessageId { get; init; }

    public long? TenantId { get; init; }

    public long? CompanyId { get; init; }

    public string? Channel { get; init; }

    public string? TemplateCode { get; init; }

    public string? Recipient { get; init; }

    public IReadOnlyDictionary<string, string>? Tokens { get; init; }

    public string? Provider { get; init; }
}
