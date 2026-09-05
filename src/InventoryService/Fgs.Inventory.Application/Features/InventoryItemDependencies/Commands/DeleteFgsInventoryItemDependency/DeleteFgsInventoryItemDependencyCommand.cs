using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.DeleteFgsInventoryItemDependency;

public sealed record DeleteFgsInventoryItemDependencyCommand(long Id) : IRequest<ApiResponse<object>>;
