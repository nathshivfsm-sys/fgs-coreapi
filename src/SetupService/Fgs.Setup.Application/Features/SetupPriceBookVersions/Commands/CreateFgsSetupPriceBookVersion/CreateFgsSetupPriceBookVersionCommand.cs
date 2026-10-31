using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.CreateFgsSetupPriceBookVersion;

public sealed record CreateFgsSetupPriceBookVersionCommand(FgsSetupPriceBookVersionCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupPriceBookVersionDetailDto>>;
