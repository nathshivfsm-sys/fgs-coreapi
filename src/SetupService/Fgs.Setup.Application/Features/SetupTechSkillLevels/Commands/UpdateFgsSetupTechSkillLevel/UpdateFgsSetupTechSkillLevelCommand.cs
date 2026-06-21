using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.UpdateFgsSetupTechSkillLevel;

public sealed record UpdateFgsSetupTechSkillLevelCommand(long Id, FgsSetupTechSkillLevelUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupTechSkillLevelDetailDto>>;
