using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.ListJobTypeSubCategories;

public sealed record ListJobTypeSubCategoriesQuery(
    SetupListQuery Query, JobTypeSubCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<JobTypeSubCategorySummaryDto>>>;
