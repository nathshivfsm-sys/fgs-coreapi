using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.CreateFgsVendorInventoryItem;

public sealed record CreateFgsVendorInventoryItemCommand(FgsVendorInventoryItemCreateDto Dto)
    : IRequest<ApiResponse<FgsVendorInventoryItemDetailDto>>;
