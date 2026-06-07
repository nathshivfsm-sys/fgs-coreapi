namespace Fgs.Contracts.CredentialAudit;

public sealed record RecordCredentialAuditRequest(
    long TenantId,
    long CompanyId,
    Guid CredentialId,
    string ActionType,
    string? Remarks = null,
    int? OldVersionNo = null,
    int? NewVersionNo = null,
    string? CreatedBy = null);
