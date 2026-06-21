using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.PatchFgsSetupTechSkillLevel;

public sealed record PatchFgsSetupTechSkillLevelCommand(long Id, FgsSetupTechSkillLevelPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupTechSkillLevelDetailDto>>;
