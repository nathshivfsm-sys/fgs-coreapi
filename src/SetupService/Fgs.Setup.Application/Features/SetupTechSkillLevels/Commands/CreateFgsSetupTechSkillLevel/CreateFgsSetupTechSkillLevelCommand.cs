using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.CreateFgsSetupTechSkillLevel;

public sealed record CreateFgsSetupTechSkillLevelCommand(FgsSetupTechSkillLevelCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupTechSkillLevelDetailDto>>;
