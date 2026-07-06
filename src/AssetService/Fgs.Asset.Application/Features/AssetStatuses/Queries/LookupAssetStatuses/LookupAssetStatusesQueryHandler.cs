using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Queries.LookupAssetStatuses;

public sealed class LookupAssetStatusesQueryHandler(IFgsAssetStatusReadRepository readRepository)
    : IRequestHandler<LookupAssetStatusesQuery, ApiResponse<IReadOnlyList<FgsAssetStatusLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetStatusLookupDto>>> Handle(
        LookupAssetStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetStatusLookupDto>>.Ok(result);
    }
}
