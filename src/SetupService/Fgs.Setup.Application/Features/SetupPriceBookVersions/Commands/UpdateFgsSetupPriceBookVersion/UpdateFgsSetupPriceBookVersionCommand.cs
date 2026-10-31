using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.UpdateFgsSetupPriceBookVersion;

public sealed record UpdateFgsSetupPriceBookVersionCommand(long Id, FgsSetupPriceBookVersionUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupPriceBookVersionDetailDto>>;
