using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Commands.PatchFgsPriceBook;

public sealed record PatchFgsPriceBookCommand(long Id, FgsPriceBookPatchDto Dto)
    : IRequest<ApiResponse<FgsPriceBookDetailDto>>;
