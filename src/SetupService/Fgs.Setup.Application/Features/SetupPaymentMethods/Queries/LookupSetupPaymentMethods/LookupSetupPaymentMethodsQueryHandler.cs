using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.LookupSetupPaymentMethods;

public sealed class LookupSetupPaymentMethodsQueryHandler(IFgsSetupPaymentMethodReadRepository readRepository)
    : IRequestHandler<LookupSetupPaymentMethodsQuery, ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>> Handle(
        LookupSetupPaymentMethodsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>(ex);
        }
    }
}
