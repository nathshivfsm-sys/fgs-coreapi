using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.ListVehicles;

public sealed class ListVehiclesQueryHandler(IFgsVehicleReadRepository readRepository)
    : IRequestHandler<ListVehiclesQuery, ApiResponse<PagedResult<FgsVehicleSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVehicleSummaryDto>>> Handle(
        ListVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsVehicleSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsVehicleSummaryDto>>(ex);
        }
    }
}
