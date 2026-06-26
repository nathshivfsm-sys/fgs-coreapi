using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListSetupTaxAuthorities;

public sealed class ListSetupTaxAuthoritiesQueryHandler(IFgsSetupTaxAuthorityReadRepository readRepository)
    : IRequestHandler<ListSetupTaxAuthoritiesQuery, ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>> Handle(
        ListSetupTaxAuthoritiesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>.Ok(result);
    }
}
