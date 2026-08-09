using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Queries.ListVendorInventoryItems;

public sealed class ListVendorInventoryItemsQueryHandler(IFgsVendorInventoryItemReadRepository readRepository)
    : IRequestHandler<ListVendorInventoryItemsQuery, ApiResponse<PagedResult<FgsVendorInventoryItemSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVendorInventoryItemSummaryDto>>> Handle(
        ListVendorInventoryItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsVendorInventoryItemSummaryDto>>.Ok(result);
    }
}
