using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Queries.GetFgsPriceBookItemById;

public sealed record GetFgsPriceBookItemByIdQuery(long Id)
    : IRequest<ApiResponse<FgsPriceBookItemDetailDto>>;
