using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.GetFgsVehicleMaintenanceById;

public sealed class GetFgsVehicleMaintenanceByIdQueryHandler(IFgsVehicleMaintenanceReadRepository readRepository)
    : IRequestHandler<GetFgsVehicleMaintenanceByIdQuery, ApiResponse<FgsVehicleMaintenanceDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleMaintenanceDetailDto>> Handle(
        GetFgsVehicleMaintenanceByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsVehicleMaintenanceDetailDto>.Fail(
                    [$"Vehicle Maintenance '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsVehicleMaintenanceDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsVehicleMaintenanceDetailDto>(ex);
        }
    }
}
