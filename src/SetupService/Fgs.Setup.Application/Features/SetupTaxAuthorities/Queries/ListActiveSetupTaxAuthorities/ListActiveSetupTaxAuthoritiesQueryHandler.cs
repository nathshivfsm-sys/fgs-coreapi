using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListActiveSetupTaxAuthorities;

public sealed class ListActiveSetupTaxAuthoritiesQueryHandler(IFgsSetupTaxAuthorityReadRepository readRepository)
    : IRequestHandler<ListActiveSetupTaxAuthoritiesQuery, ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>> Handle(
        ListActiveSetupTaxAuthoritiesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupTaxAuthorityListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTaxAuthoritySummaryDto>>(ex);
        }
    }
}
