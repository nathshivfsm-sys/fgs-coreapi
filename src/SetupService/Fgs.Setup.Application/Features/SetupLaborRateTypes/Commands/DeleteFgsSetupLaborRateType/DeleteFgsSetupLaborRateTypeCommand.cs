using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.DeleteFgsSetupLaborRateType;

public sealed record DeleteFgsSetupLaborRateTypeCommand(long Id)
    : IRequest<ApiResponse<FgsSetupLaborRateTypeDetailDto>>;
