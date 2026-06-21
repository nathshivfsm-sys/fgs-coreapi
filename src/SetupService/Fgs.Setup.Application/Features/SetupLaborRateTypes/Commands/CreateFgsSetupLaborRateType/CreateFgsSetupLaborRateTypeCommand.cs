using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.CreateFgsSetupLaborRateType;

public sealed record CreateFgsSetupLaborRateTypeCommand(FgsSetupLaborRateTypeCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupLaborRateTypeDetailDto>>;
