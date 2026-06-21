using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.ListActiveSetupTaxDetails;

public sealed record ListActiveSetupTaxDetailsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupTaxDetailListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>>;
