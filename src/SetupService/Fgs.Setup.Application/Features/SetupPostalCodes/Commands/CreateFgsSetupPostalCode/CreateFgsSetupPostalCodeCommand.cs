using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.CreateFgsSetupPostalCode;

public sealed record CreateFgsSetupPostalCodeCommand(FgsSetupPostalCodeCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupPostalCodeDetailDto>>;
