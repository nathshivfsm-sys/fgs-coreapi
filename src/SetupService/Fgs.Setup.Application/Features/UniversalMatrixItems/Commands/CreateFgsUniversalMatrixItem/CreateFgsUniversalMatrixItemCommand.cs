using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.CreateFgsUniversalMatrixItem;

public sealed record CreateFgsUniversalMatrixItemCommand(FgsUniversalMatrixItemCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixItemDetailDto>>;
