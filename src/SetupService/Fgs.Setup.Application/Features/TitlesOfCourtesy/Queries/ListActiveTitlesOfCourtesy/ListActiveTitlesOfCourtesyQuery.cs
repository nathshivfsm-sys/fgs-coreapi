using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListActiveTitlesOfCourtesy;

public sealed record ListActiveTitlesOfCourtesyQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    TitleOfCourtesyListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>>;
