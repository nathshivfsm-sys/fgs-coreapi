using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.PatchFgsSetupDescription;

public sealed record PatchFgsSetupDescriptionCommand(long Id, FgsSetupDescriptionPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupDescriptionDetailDto>>;
