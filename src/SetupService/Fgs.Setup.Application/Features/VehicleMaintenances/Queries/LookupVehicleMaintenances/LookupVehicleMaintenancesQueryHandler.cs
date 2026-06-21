using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.LookupVehicleMaintenances;

public sealed class LookupVehicleMaintenancesQueryHandler(IFgsVehicleMaintenanceReadRepository readRepository)
    : IRequestHandler<LookupVehicleMaintenancesQuery, ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>> Handle(
        LookupVehicleMaintenancesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>(ex);
        }
    }
}
