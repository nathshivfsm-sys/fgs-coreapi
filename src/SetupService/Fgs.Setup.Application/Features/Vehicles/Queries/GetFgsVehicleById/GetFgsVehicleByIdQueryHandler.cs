using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.GetFgsVehicleById;

public sealed class GetFgsVehicleByIdQueryHandler(IFgsVehicleReadRepository readRepository)
    : IRequestHandler<GetFgsVehicleByIdQuery, ApiResponse<FgsVehicleDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleDetailDto>> Handle(
        GetFgsVehicleByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsVehicleDetailDto>.Fail(
                    [$"Vehicle '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsVehicleDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsVehicleDetailDto>(ex);
        }
    }
}
