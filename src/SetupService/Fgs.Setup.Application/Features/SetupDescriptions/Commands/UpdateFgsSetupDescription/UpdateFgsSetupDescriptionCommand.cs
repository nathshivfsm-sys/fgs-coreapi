using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.UpdateFgsSetupDescription;

public sealed record UpdateFgsSetupDescriptionCommand(long Id, FgsSetupDescriptionUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupDescriptionDetailDto>>;
