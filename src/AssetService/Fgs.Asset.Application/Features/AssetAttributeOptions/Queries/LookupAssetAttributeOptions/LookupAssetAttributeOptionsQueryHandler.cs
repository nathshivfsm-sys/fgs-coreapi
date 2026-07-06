using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Queries.LookupAssetAttributeOptions;

public sealed class LookupAssetAttributeOptionsQueryHandler(IFgsAssetAttributeOptionReadRepository readRepository)
    : IRequestHandler<LookupAssetAttributeOptionsQuery, ApiResponse<IReadOnlyList<FgsAssetAttributeOptionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetAttributeOptionLookupDto>>> Handle(
        LookupAssetAttributeOptionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetAttributeOptionLookupDto>>.Ok(result);
    }
}
