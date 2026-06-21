using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.UpdateFgsSetupLaborRateType;

public sealed record UpdateFgsSetupLaborRateTypeCommand(long Id, FgsSetupLaborRateTypeUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupLaborRateTypeDetailDto>>;
