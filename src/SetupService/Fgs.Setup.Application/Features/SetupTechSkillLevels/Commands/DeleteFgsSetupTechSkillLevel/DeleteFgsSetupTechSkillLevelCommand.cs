using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.DeleteFgsSetupTechSkillLevel;

public sealed record DeleteFgsSetupTechSkillLevelCommand(long Id)
    : IRequest<ApiResponse<FgsSetupTechSkillLevelDetailDto>>;
