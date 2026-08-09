using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Enums;
using Fgs.Audit.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Audit.Infrastructure.Audit;

public sealed class AuditEventReadRepository(FgsAuditDbContext context) : IAuditEventReadRepository
{
    public async Task<AuditEventDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await context.FgsEvents
            .AsNoTracking()
            .Include(e => e.Details)
            .Include(e => e.Attachments)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : AuditEventMapper.ToDetailDto(entity);
    }

    public async Task<IReadOnlyList<AuditEventSummaryDto>> ListByEntityAsync(
        AuditRecordType recordType,
        long entityId,
        long? tenantId = null,
        long? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.FgsEvents
            .AsNoTracking()
            .Where(e => e.RecordType == recordType && e.EntityId == entityId);

        if (tenantId.HasValue)
        {
            query = query.Where(e => e.TenantId == tenantId.Value);
        }

        if (companyId.HasValue)
        {
            query = query.Where(e => e.CompanyId == companyId.Value);
        }

        var entities = await query
            .OrderByDescending(e => e.OccurredOn)
            .ThenByDescending(e => e.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(AuditEventMapper.ToSummaryDto).ToList();
    }
}
