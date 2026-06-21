using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Queries.LookupVendors;

public sealed class LookupVendorsQueryHandler(IFgsVendorReadRepository readRepository)
    : IRequestHandler<LookupVendorsQuery, ApiResponse<IReadOnlyList<FgsVendorLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsVendorLookupDto>>> Handle(
        LookupVendorsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsVendorLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsVendorLookupDto>>(ex);
        }
    }
}
