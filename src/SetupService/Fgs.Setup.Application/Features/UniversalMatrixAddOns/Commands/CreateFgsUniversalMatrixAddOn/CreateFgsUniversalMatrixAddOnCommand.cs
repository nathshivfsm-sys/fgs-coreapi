using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.CreateFgsUniversalMatrixAddOn;

public sealed record CreateFgsUniversalMatrixAddOnCommand(FgsUniversalMatrixAddOnCreateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixAddOnDetailDto>>;
