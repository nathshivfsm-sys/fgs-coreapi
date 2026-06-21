using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.PatchFgsSetupLaborRateType;

public sealed record PatchFgsSetupLaborRateTypeCommand(long Id, FgsSetupLaborRateTypePatchDto Dto)
    : IRequest<ApiResponse<FgsSetupLaborRateTypeDetailDto>>;
