using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.UpdateFgsInventoryItemAlternates;

public sealed record UpdateFgsInventoryItemAlternatesCommand(FgsInventoryItemAlternateReplaceDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>>;
