using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.PatchFgsSetupPriceBookVersion;

public sealed record PatchFgsSetupPriceBookVersionCommand(long Id, FgsSetupPriceBookVersionPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupPriceBookVersionDetailDto>>;
