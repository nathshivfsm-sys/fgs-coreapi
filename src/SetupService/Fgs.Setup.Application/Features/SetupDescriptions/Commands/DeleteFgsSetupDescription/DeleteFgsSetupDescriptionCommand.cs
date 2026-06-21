using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.DeleteFgsSetupDescription;

public sealed record DeleteFgsSetupDescriptionCommand(long Id)
    : IRequest<ApiResponse<FgsSetupDescriptionDetailDto>>;
