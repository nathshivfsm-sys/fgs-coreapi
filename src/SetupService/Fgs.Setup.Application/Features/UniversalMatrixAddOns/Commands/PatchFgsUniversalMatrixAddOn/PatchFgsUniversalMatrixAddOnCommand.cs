using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.PatchFgsUniversalMatrixAddOn;

public sealed record PatchFgsUniversalMatrixAddOnCommand(long Id, FgsUniversalMatrixAddOnPatchDto Dto)
    : IRequest<ApiResponse<FgsUniversalMatrixAddOnDetailDto>>;
