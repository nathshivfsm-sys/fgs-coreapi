using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.DeleteFgsUniversalMatrixAddOn;

public sealed record DeleteFgsUniversalMatrixAddOnCommand(long Id)
    : IRequest<ApiResponse<FgsUniversalMatrixAddOnDetailDto>>;
