using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Queries.LookupInventorySerials;

public sealed record LookupInventorySerialsQuery(long? InventoryItemId = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventorySerialLookupDto>>>;
