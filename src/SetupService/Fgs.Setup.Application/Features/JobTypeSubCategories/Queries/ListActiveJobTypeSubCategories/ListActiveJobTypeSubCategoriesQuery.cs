using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.ListActiveJobTypeSubCategories;

public sealed record ListActiveJobTypeSubCategoriesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, JobTypeSubCategoryListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>>;
