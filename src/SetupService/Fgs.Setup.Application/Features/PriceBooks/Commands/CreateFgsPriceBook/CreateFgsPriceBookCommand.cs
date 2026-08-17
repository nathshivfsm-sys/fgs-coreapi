using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Commands.CreateFgsPriceBook;

public sealed record CreateFgsPriceBookCommand(FgsPriceBookCreateDto Dto)
    : IRequest<ApiResponse<FgsPriceBookDetailDto>>;
