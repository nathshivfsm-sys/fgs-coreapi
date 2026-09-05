using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Commands.DeleteFgsPriceBookItem;

public sealed record DeleteFgsPriceBookItemCommand(long Id)
    : IRequest<ApiResponse<FgsPriceBookItemDetailDto>>;
