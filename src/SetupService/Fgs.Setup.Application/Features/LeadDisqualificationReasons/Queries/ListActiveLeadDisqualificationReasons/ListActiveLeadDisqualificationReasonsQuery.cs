using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListActiveLeadDisqualificationReasons;

public sealed record ListActiveLeadDisqualificationReasonsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, LeadDisqualificationReasonListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>>;
