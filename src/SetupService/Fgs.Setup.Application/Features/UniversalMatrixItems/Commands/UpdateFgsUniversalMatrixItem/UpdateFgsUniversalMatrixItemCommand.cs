using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.UpdateFgsUniversalMatrixItem;

public sealed record UpdateFgsUniversalMatrixItemCommand(long Id, FgsUniversalMatrixItemUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixItemDetailDto>>;
