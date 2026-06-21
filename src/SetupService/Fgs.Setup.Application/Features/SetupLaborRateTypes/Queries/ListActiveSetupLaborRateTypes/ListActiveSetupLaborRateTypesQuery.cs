using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.ListActiveSetupLaborRateTypes;

public sealed record ListActiveSetupLaborRateTypesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupLaborRateTypeListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>>;
