using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.ListActiveSetupZones;

public sealed record ListActiveSetupZonesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupZoneListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>>;
