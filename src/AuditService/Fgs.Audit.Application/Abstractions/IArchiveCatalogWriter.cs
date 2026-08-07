using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Audit;

namespace Fgs.Audit.Application.Abstractions;

public interface IArchiveCatalogWriter
{
    /// <summary>
    /// Creates or updates by archive month. Returns the DTO and whether a new row was created.
    /// </summary>
    Task<(ArchiveCatalogDto Dto, bool Created)> UpsertAsync(
        UpsertArchiveCatalogRequest request,
        CancellationToken cancellationToken = default);
}
