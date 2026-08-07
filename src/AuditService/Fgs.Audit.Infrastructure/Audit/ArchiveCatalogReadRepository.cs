using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Audit.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Audit.Infrastructure.Audit;

public sealed class ArchiveCatalogReadRepository(FgsAuditDbContext context) : IArchiveCatalogReadRepository
{
    public async Task<ArchiveCatalogDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await context.FgsArchiveCatalogs
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return entity is null ? null : ArchiveCatalogMapper.ToDto(entity);
    }

    public async Task<IReadOnlyList<ArchiveCatalogDto>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await context.FgsArchiveCatalogs
            .AsNoTracking()
            .OrderByDescending(a => a.ArchiveMonth)
            .ThenByDescending(a => a.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(ArchiveCatalogMapper.ToDto).ToList();
    }
}
