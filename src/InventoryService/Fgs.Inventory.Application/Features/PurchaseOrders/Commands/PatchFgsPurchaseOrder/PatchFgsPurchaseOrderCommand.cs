using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Commands.PatchFgsPurchaseOrder;

public sealed record PatchFgsPurchaseOrderCommand(long Id, FgsPurchaseOrderPatchDto Dto)
    : IRequest<ApiResponse<FgsPurchaseOrderDetailDto>>;
