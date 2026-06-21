using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.LookupVehicles;

public sealed class LookupVehiclesQueryHandler(IFgsVehicleReadRepository readRepository)
    : IRequestHandler<LookupVehiclesQuery, ApiResponse<IReadOnlyList<FgsVehicleLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsVehicleLookupDto>>> Handle(
        LookupVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsVehicleLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsVehicleLookupDto>>(ex);
        }
    }
}
