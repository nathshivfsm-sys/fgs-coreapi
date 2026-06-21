using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.CreateFgsSetupDescription;

public sealed record CreateFgsSetupDescriptionCommand(FgsSetupDescriptionCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupDescriptionDetailDto>>;
