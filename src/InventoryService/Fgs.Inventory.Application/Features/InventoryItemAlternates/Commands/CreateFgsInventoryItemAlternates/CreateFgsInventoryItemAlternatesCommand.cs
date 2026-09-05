using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.CreateFgsInventoryItemAlternates;

public sealed record CreateFgsInventoryItemAlternatesCommand(FgsInventoryItemAlternateReplaceDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>>;
