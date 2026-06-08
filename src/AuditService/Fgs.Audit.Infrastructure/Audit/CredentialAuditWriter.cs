using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Domain.Entities;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.CredentialAudit;

namespace Fgs.Audit.Infrastructure.Audit;

public sealed class CredentialAuditWriter(FgsAuditDbContext context) : ICredentialAuditWriter
{
    public async Task WriteAsync(RecordCredentialAuditRequest request, CancellationToken cancellationToken = default)
    {
        var audit = new FgsCredentialAudit
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            CompanyId = request.CompanyId,
            CredentialId = request.CredentialId,
            ActionType = request.ActionType,
            Remarks = request.Remarks,
            OldVersionNo = request.OldVersionNo,
            NewVersionNo = request.NewVersionNo,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = request.CreatedBy
        };

        await context.FgsCredentialAudits.AddAsync(audit, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
