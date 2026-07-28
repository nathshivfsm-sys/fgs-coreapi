using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.ListJobCategories;

public sealed record ListJobCategoriesQuery(
    SetupListQuery Query, JobCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<JobCategorySummaryDto>>>;
