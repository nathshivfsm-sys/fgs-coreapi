using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Queries.GetFgsPriceBookById;

public sealed record GetFgsPriceBookByIdQuery(long Id)
    : IRequest<ApiResponse<FgsPriceBookDetailDto>>;
