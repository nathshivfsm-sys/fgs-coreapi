using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.DeleteFgsUniversalMatrixItem;

public sealed record DeleteFgsUniversalMatrixItemCommand(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixItemDetailDto>>;
