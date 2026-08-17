using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Queries.GetFgsInventoryItemAlternateById;

public sealed record GetFgsInventoryItemAlternateByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryItemAlternateDetailDto>>;
