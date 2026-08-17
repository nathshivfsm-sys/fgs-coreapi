using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Queries.ListFgsInventoryItemAlternates;

public sealed record ListFgsInventoryItemAlternatesQuery(long InventoryItemId)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>>;
