using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Queries.LookupAssetAttributes;

public sealed class LookupAssetAttributesQueryHandler(IFgsAssetAttributeReadRepository readRepository)
    : IRequestHandler<LookupAssetAttributesQuery, ApiResponse<IReadOnlyList<FgsAssetAttributeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetAttributeLookupDto>>> Handle(
        LookupAssetAttributesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetAttributeLookupDto>>.Ok(result);
    }
}
