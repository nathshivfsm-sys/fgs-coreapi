using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.PatchFgsVendorInventoryItem;

public sealed record PatchFgsVendorInventoryItemCommand(long Id, FgsVendorInventoryItemPatchDto Dto)
    : IRequest<ApiResponse<FgsVendorInventoryItemDetailDto>>;
