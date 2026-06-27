using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListSetupTaxAuthorities;

public sealed record ListSetupTaxAuthoritiesQuery(
    SetupListQuery Query, FgsSetupTaxAuthorityListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>>;
