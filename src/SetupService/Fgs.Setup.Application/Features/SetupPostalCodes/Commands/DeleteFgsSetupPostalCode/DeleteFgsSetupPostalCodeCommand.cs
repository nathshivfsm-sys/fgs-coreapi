using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.DeleteFgsSetupPostalCode;

public sealed record DeleteFgsSetupPostalCodeCommand(long Id)
    : IRequest<ApiResponse<FgsSetupPostalCodeDetailDto>>;
