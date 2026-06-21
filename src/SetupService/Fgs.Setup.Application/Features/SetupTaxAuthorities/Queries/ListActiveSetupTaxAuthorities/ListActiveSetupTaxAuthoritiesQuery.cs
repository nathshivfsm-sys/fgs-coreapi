using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListActiveSetupTaxAuthorities;

public sealed record ListActiveSetupTaxAuthoritiesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupTaxAuthorityListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>>;
