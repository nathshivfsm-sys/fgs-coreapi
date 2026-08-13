using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Commands.CreateFgsPriceBookItem;

public sealed record CreateFgsPriceBookItemCommand(FgsPriceBookItemCreateDto Dto)
    : IRequest<ApiResponse<FgsPriceBookItemDetailDto>>;
