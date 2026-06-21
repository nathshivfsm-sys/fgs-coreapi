using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.ListActiveLeadStatuses;

public sealed record ListActiveLeadStatusesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, LeadStatusListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<LeadStatusSummaryDto>>>;
