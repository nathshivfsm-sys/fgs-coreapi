using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Commands.PatchFgsPriceBookItem;

public sealed record PatchFgsPriceBookItemCommand(long Id, FgsPriceBookItemPatchDto Dto)
    : IRequest<ApiResponse<FgsPriceBookItemDetailDto>>;
