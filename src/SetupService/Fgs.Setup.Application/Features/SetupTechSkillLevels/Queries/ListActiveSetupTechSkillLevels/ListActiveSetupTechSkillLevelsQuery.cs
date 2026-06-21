using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.ListActiveSetupTechSkillLevels;

public sealed record ListActiveSetupTechSkillLevelsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupTechSkillLevelListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupTechSkillLevelSummaryDto>>>;
