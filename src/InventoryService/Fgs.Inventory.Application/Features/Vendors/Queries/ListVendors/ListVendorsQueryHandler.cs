using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Queries.ListVendors;

public sealed class ListVendorsQueryHandler(IFgsVendorReadRepository readRepository)
    : IRequestHandler<ListVendorsQuery, ApiResponse<PagedResult<FgsVendorSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVendorSummaryDto>>> Handle(
        ListVendorsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsVendorSummaryDto>>.Ok(result);
    }
}
