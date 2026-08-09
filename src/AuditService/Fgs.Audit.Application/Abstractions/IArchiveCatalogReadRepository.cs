using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;

namespace Fgs.Audit.Application.Abstractions;

public interface IArchiveCatalogReadRepository
{
    Task<ArchiveCatalogDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ArchiveCatalogDto>> ListAsync(CancellationToken cancellationToken = default);
}
