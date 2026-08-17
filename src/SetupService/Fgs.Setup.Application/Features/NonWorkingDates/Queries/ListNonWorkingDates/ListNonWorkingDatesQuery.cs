using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Queries.ListNonWorkingDates;

public sealed record ListNonWorkingDatesQuery(
    SetupListQuery Query,
    FgsNonWorkingDateListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsNonWorkingDateSummaryDto>>>;
