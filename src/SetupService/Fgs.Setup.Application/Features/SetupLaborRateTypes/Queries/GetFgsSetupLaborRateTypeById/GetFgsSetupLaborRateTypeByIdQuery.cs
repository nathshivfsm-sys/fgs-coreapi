using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.GetFgsSetupLaborRateTypeById;

public sealed record GetFgsSetupLaborRateTypeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupLaborRateTypeDetailDto>>;
