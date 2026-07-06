using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Queries.LookupAssets;

public sealed class LookupAssetsQueryHandler(IFgsAssetReadRepository readRepository)
    : IRequestHandler<LookupAssetsQuery, ApiResponse<IReadOnlyList<FgsAssetLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetLookupDto>>> Handle(
        LookupAssetsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetLookupDto>>.Ok(result);
    }
}
