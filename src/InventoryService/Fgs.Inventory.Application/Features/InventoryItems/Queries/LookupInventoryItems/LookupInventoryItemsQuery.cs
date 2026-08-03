using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Queries.LookupInventoryItems;

public sealed record LookupInventoryItemsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemLookupDto>>>;
