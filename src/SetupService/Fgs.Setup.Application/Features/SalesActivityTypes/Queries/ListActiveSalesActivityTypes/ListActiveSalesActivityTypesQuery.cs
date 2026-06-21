using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListActiveSalesActivityTypes;

public sealed record ListActiveSalesActivityTypesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSalesActivityTypeListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>>;
