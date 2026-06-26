using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.ListSetupTechSkillLevels;

public sealed record ListSetupTechSkillLevelsQuery(
    SetupListQuery Query, FgsSetupTechSkillLevelListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>>;
