namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialAuditWriter
{
    Task WriteAsync(
        long tenantId,
        long companyId,
        Guid credentialSecretId,
        string actionType,
        int? oldVersionNo,
        int? newVersionNo,
        string? remarks,
        string? createdBy,
        bool saveImmediately = true,
        CancellationToken cancellationToken = default);
}
