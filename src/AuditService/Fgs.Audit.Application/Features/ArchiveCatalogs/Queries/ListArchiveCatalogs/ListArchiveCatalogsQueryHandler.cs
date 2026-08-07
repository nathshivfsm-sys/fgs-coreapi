using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.ListArchiveCatalogs;

public sealed class ListArchiveCatalogsQueryHandler(IArchiveCatalogReadRepository readRepository)
    : IRequestHandler<ListArchiveCatalogsQuery, ApiResponse<IReadOnlyList<ArchiveCatalogDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<ArchiveCatalogDto>>> Handle(
        ListArchiveCatalogsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<ArchiveCatalogDto>>.Ok(result);
    }
}
