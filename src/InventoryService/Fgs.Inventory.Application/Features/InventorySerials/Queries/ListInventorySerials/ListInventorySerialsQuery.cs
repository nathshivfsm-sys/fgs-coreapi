using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Queries.ListInventorySerials;

public sealed record ListInventorySerialsQuery(
    InventoryListQuery Query,
    FgsInventorySerialListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventorySerialSummaryDto>>>;
