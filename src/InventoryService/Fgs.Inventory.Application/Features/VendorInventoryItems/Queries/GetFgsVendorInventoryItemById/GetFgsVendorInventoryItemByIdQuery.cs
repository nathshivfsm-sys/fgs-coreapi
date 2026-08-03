using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Queries.GetFgsVendorInventoryItemById;

public sealed record GetFgsVendorInventoryItemByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVendorInventoryItemDetailDto>>;
