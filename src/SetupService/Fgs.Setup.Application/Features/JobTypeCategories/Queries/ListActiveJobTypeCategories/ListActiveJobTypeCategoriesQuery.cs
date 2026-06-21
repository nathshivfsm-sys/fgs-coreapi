using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.ListActiveJobTypeCategories;

public sealed record ListActiveJobTypeCategoriesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, JobTypeCategoryListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<JobTypeCategorySummaryDto>>>;
