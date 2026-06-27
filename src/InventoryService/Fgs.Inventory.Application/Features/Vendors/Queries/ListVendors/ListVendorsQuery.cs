using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Queries.ListVendors;

public sealed record ListVendorsQuery(
    InventoryListQuery Query,
    FgsVendorListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVendorSummaryDto>>>;
