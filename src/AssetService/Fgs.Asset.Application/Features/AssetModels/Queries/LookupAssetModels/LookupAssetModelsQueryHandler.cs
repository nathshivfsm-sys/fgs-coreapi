using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Queries.LookupAssetModels;

public sealed class LookupAssetModelsQueryHandler(IFgsAssetModelReadRepository readRepository)
    : IRequestHandler<LookupAssetModelsQuery, ApiResponse<IReadOnlyList<FgsAssetModelLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetModelLookupDto>>> Handle(
        LookupAssetModelsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetModelLookupDto>>.Ok(result);
    }
}
