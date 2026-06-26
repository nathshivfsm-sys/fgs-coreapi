using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.ListSetupZones;

public sealed record ListSetupZonesQuery(
    SetupListQuery Query, FgsSetupZoneListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>>;
