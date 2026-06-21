using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.ListActiveJobTypes;

public sealed record ListActiveJobTypesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, JobTypeListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<JobTypeSummaryDto>>>;
