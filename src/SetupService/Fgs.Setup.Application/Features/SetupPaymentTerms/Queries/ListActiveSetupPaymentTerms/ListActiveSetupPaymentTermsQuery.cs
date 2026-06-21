using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.ListActiveSetupPaymentTerms;

public sealed record ListActiveSetupPaymentTermsQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupPaymentTermListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>>;
