using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.GetFgsSetupPostalCodeById;

public sealed record GetFgsSetupPostalCodeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupPostalCodeDetailDto>>;
