using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Commands.CreateFgsPurchaseOrder;

public sealed record CreateFgsPurchaseOrderCommand(FgsPurchaseOrderCreateDto Dto)
    : IRequest<ApiResponse<FgsPurchaseOrderDetailDto>>;
