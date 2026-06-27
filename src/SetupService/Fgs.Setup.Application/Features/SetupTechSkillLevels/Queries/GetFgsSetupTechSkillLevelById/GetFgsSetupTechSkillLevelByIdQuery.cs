using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.GetFgsSetupTechSkillLevelById;

public sealed record GetFgsSetupTechSkillLevelByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupTechSkillLevelDetailDto>>;
