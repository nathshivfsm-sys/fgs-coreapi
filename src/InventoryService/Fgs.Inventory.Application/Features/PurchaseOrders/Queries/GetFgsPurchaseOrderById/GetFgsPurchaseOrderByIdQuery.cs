using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Queries.GetFgsPurchaseOrderById;

public sealed record GetFgsPurchaseOrderByIdQuery(long Id)
    : IRequest<ApiResponse<FgsPurchaseOrderDetailDto>>;
