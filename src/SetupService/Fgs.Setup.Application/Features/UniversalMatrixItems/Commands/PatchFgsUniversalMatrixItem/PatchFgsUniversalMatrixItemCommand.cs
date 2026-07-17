using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.PatchFgsUniversalMatrixItem;

public sealed record PatchFgsUniversalMatrixItemCommand(long Id, FgsUniversalMatrixItemPatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixItemDetailDto>>;
