using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Queries.LookupAssetWarranties;

public sealed class LookupAssetWarrantiesQueryHandler(IFgsAssetWarrantyReadRepository readRepository)
    : IRequestHandler<LookupAssetWarrantiesQuery, ApiResponse<IReadOnlyList<FgsAssetWarrantyLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetWarrantyLookupDto>>> Handle(
        LookupAssetWarrantiesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetWarrantyLookupDto>>.Ok(result);
    }
}
