using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class CredentialAuditWriter(
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTime) : ICredentialAuditWriter
{
    public async Task WriteAsync(
        long tenantId,
        long companyId,
        Guid credentialSecretId,
        string actionType,
        int? oldVersionNo,
        int? newVersionNo,
        string? remarks,
        string? createdBy,
        bool saveImmediately = true,
        CancellationToken cancellationToken = default)
    {
        var audit = new FgsCredentialAudit
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CompanyId = companyId,
            CredentialSecretId = credentialSecretId,
            ActionType = actionType,
            OldVersionNo = oldVersionNo,
            NewVersionNo = newVersionNo,
            Remarks = remarks,
            CreatedOn = dateTime.UtcNow,
            CreatedBy = createdBy
        };

        await unitOfWork.Repository<FgsCredentialAudit>().AddAsync(audit, cancellationToken);

        if (saveImmediately)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
