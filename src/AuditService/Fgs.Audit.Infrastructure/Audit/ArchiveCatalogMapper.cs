using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Audit.Domain.Entities;

namespace Fgs.Audit.Infrastructure.Audit;

internal static class ArchiveCatalogMapper
{
    public static ArchiveCatalogDto ToDto(FgsArchiveCatalog entity) =>
        new(
            entity.Id,
            entity.ArchiveMonth,
            entity.StoragePath,
            entity.FileSize,
            entity.CreatedOn);
}
