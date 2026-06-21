using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.LookupSetupTaxAuthorities;

public sealed class LookupSetupTaxAuthoritiesQueryHandler(IFgsSetupTaxAuthorityReadRepository readRepository)
    : IRequestHandler<LookupSetupTaxAuthoritiesQuery, ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>> Handle(
        LookupSetupTaxAuthoritiesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>(ex);
        }
    }
}
