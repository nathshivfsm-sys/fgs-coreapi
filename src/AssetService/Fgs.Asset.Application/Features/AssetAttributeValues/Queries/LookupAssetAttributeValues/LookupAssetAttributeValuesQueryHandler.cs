using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Queries.LookupAssetAttributeValues;

public sealed class LookupAssetAttributeValuesQueryHandler(IFgsAssetAttributeValueReadRepository readRepository)
    : IRequestHandler<LookupAssetAttributeValuesQuery, ApiResponse<IReadOnlyList<FgsAssetAttributeValueLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetAttributeValueLookupDto>>> Handle(
        LookupAssetAttributeValuesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetAttributeValueLookupDto>>.Ok(result);
    }
}
