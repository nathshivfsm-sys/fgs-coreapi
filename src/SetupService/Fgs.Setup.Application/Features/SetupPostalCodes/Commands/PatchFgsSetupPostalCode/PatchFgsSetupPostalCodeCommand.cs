using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.PatchFgsSetupPostalCode;

public sealed record PatchFgsSetupPostalCodeCommand(long Id, FgsSetupPostalCodePatchDto Dto)
    : IRequest<ApiResponse<FgsSetupPostalCodeDetailDto>>;
