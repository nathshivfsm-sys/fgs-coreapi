using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.UpdateFgsVendorInventoryItem;

public sealed record UpdateFgsVendorInventoryItemCommand(long Id, FgsVendorInventoryItemUpdateDto Dto)
    : IRequest<ApiResponse<FgsVendorInventoryItemDetailDto>>;
