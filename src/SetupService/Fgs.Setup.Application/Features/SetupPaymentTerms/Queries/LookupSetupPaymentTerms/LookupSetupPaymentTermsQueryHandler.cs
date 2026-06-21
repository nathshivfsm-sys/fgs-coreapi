using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.LookupSetupPaymentTerms;

public sealed class LookupSetupPaymentTermsQueryHandler(IFgsSetupPaymentTermReadRepository readRepository)
    : IRequestHandler<LookupSetupPaymentTermsQuery, ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>> Handle(
        LookupSetupPaymentTermsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupPaymentTermLookupDto>>(ex);
        }
    }
}
