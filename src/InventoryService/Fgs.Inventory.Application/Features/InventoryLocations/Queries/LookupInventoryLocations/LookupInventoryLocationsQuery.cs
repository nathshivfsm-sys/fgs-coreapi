using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.LookupInventoryLocations;

public sealed record LookupInventoryLocationsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryLocationLookupDto>>>;
