namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Published when a credential audit record should be written to the Audit service.
/// </summary>
public sealed record CredentialAuditRequestedEvent(
    long TenantId,
    long CompanyId,
    Guid CredentialId,
    string ActionType,
    string? Remarks = null,
    int? OldVersionNo = null,
    int? NewVersionNo = null,
    string? CreatedBy = null);
