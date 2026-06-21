using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.LookupSetupPostalCodes;

public sealed class LookupSetupPostalCodesQueryHandler(IFgsSetupPostalCodeReadRepository readRepository)
    : IRequestHandler<LookupSetupPostalCodesQuery, ApiResponse<IReadOnlyList<FgsSetupPostalCodeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPostalCodeLookupDto>>> Handle(
        LookupSetupPostalCodesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupPostalCodeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupPostalCodeLookupDto>>(ex);
        }
    }
}
