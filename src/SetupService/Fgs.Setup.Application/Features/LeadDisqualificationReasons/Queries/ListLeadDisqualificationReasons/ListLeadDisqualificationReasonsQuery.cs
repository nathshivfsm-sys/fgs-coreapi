using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListLeadDisqualificationReasons;

public sealed record ListLeadDisqualificationReasonsQuery(
    SetupListQuery Query, LeadDisqualificationReasonListFilters Filters)
    : IRequest<ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>>;
