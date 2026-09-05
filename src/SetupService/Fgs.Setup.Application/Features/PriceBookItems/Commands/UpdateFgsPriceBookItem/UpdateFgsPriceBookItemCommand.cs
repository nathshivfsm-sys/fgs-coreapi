using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Commands.UpdateFgsPriceBookItem;

public sealed record UpdateFgsPriceBookItemCommand(long Id, FgsPriceBookItemUpdateDto Dto)
    : IRequest<ApiResponse<FgsPriceBookItemDetailDto>>;
