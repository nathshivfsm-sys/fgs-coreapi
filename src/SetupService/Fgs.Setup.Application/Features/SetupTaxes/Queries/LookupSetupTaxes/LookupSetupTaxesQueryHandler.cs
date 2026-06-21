using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.LookupSetupTaxes;

public sealed class LookupSetupTaxesQueryHandler(IFgsSetupTaxReadRepository readRepository)
    : IRequestHandler<LookupSetupTaxesQuery, ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>> Handle(
        LookupSetupTaxesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupTaxLookupDto>>(ex);
        }
    }
}
