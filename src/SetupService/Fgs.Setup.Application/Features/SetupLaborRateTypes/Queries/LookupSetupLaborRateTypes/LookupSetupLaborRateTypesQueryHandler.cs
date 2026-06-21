using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.LookupSetupLaborRateTypes;

public sealed class LookupSetupLaborRateTypesQueryHandler(IFgsSetupLaborRateTypeReadRepository readRepository)
    : IRequestHandler<LookupSetupLaborRateTypesQuery, ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>> Handle(
        LookupSetupLaborRateTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>(ex);
        }
    }
}
