using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.ListSetupTimeSlots;

public sealed record ListSetupTimeSlotsQuery(
    SetupListQuery Query, FgsSetupTimeSlotListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>>;
