using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.LookupInventoryItemTypes;

public sealed record LookupInventoryItemTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemTypeLookupDto>>>;
