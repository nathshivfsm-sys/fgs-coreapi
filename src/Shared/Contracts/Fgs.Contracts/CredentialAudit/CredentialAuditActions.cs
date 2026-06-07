namespace Fgs.Contracts.CredentialAudit;

public static class CredentialAuditActions
{
    public const string Created = "CREATED";
    public const string Updated = "UPDATED";
    public const string Rotated = "ROTATED";
    public const string Revoked = "REVOKED";
    public const string SecretAccessed = "SECRET_ACCESSED";
    public const string SecretAccessDenied = "SECRET_ACCESS_DENIED";
}
