using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Queries.LookupAssetManufacturers;

public sealed class LookupAssetManufacturersQueryHandler(IFgsAssetManufacturerReadRepository readRepository)
    : IRequestHandler<LookupAssetManufacturersQuery, ApiResponse<IReadOnlyList<FgsAssetManufacturerLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsAssetManufacturerLookupDto>>> Handle(
        LookupAssetManufacturersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsAssetManufacturerLookupDto>>.Ok(result);
    }
}
