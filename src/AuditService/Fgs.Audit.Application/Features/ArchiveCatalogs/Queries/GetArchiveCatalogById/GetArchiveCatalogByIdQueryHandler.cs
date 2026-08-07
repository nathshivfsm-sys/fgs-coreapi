using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.GetArchiveCatalogById;

public sealed class GetArchiveCatalogByIdQueryHandler(IArchiveCatalogReadRepository readRepository)
    : IRequestHandler<GetArchiveCatalogByIdQuery, ApiResponse<ArchiveCatalogDto>>
{
    public async Task<ApiResponse<ArchiveCatalogDto>> Handle(
        GetArchiveCatalogByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<ArchiveCatalogDto>.Fail(
                [$"Archive catalog '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<ArchiveCatalogDto>.Ok(result);
    }
}
