using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Queries.LookupAssetTypes;

public sealed class LookupAssetTypesQueryHandler(IFgsAssetTypeReadRepository readRepository)
    : IRequestHandler<LookupAssetTypesQuery, ApiResponse<IReadOnlyList<FgsAssetTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetTypeLookupDto>>> Handle(
        LookupAssetTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetTypeLookupDto>>.Ok(result);
    }
}
