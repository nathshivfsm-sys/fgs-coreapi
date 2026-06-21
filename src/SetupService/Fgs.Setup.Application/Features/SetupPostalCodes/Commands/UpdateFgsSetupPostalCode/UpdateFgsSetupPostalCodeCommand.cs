using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.UpdateFgsSetupPostalCode;

public sealed record UpdateFgsSetupPostalCodeCommand(long Id, FgsSetupPostalCodeUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupPostalCodeDetailDto>>;
