using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.LookupSetupTechSkillLevels;

public sealed record LookupSetupTechSkillLevelsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>>;
