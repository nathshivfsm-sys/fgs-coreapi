using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.LookupSetupZones;

public sealed class LookupSetupZonesQueryHandler(IFgsSetupZoneReadRepository readRepository)
    : IRequestHandler<LookupSetupZonesQuery, ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>> Handle(
        LookupSetupZonesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupZoneLookupDto>>(ex);
        }
    }
}
