using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Commands.UpdateFgsPriceBook;

public sealed record UpdateFgsPriceBookCommand(long Id, FgsPriceBookUpdateDto Dto)
    : IRequest<ApiResponse<FgsPriceBookDetailDto>>;
