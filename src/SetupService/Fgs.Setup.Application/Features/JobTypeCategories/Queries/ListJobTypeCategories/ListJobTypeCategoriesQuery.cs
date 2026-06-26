using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.ListJobTypeCategories;

public sealed record ListJobTypeCategoriesQuery(
    SetupListQuery Query, JobTypeCategoryListFilters Filters)
    : IRequest<ApiResponse<PagedResult<JobTypeCategorySummaryDto>>>;
