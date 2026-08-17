using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.DeleteFgsInventoryItemAlternate;

public sealed record DeleteFgsInventoryItemAlternateCommand(long Id) : IRequest<ApiResponse<object>>;
