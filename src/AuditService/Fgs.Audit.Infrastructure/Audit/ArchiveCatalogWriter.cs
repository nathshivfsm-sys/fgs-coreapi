using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Audit.Domain.Entities;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.Audit;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Audit.Infrastructure.Audit;

public sealed class ArchiveCatalogWriter(FgsAuditDbContext context) : IArchiveCatalogWriter
{
    public async Task<(ArchiveCatalogDto Dto, bool Created)> UpsertAsync(
        UpsertArchiveCatalogRequest request,
        CancellationToken cancellationToken = default)
    {
        var archiveMonth = new DateOnly(request.ArchiveMonth.Year, request.ArchiveMonth.Month, 1);
        var existing = await context.FgsArchiveCatalogs
            .FirstOrDefaultAsync(a => a.ArchiveMonth == archiveMonth, cancellationToken);

        var created = existing is null;
        if (existing is null)
        {
            existing = new FgsArchiveCatalog
            {
                ArchiveMonth = archiveMonth,
                StoragePath = request.StoragePath.Trim(),
                FileSize = request.FileSize,
                CreatedOn = DateTime.UtcNow
            };
            await context.FgsArchiveCatalogs.AddAsync(existing, cancellationToken);
        }
        else
        {
            existing.StoragePath = request.StoragePath.Trim();
            existing.FileSize = request.FileSize;
        }

        await context.SaveChangesAsync(cancellationToken);
        return (ArchiveCatalogMapper.ToDto(existing), created);
    }
}
