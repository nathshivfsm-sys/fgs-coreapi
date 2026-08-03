using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.UpdateFgsUniversalMatrixAddOn;

public sealed record UpdateFgsUniversalMatrixAddOnCommand(long Id, FgsUniversalMatrixAddOnUpdateDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixAddOnDetailDto>>;
