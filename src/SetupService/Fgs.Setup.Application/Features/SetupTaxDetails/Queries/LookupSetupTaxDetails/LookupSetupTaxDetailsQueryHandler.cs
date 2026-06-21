using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.LookupSetupTaxDetails;

public sealed class LookupSetupTaxDetailsQueryHandler(IFgsSetupTaxDetailReadRepository readRepository)
    : IRequestHandler<LookupSetupTaxDetailsQuery, ApiResponse<IReadOnlyList<FgsSetupTaxDetailLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTaxDetailLookupDto>>> Handle(
        LookupSetupTaxDetailsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupTaxDetailLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupTaxDetailLookupDto>>(ex);
        }
    }
}
