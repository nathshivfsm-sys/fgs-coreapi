using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.ListLeadStatuses;

public sealed record ListLeadStatusesQuery(
    SetupListQuery Query, LeadStatusListFilters Filters)
    : IRequest<ApiResponse<PagedResult<LeadStatusSummaryDto>>>;
