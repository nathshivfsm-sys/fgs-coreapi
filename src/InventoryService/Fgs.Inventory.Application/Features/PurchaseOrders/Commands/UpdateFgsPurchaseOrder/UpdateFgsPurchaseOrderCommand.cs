using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Commands.UpdateFgsPurchaseOrder;

public sealed record UpdateFgsPurchaseOrderCommand(long Id, FgsPurchaseOrderUpdateDto Dto)
    : IRequest<ApiResponse<FgsPurchaseOrderDetailDto>>;
