using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.ListActiveVehicles;

public sealed class ListActiveVehiclesQueryHandler(IFgsVehicleReadRepository readRepository)
    : IRequestHandler<ListActiveVehiclesQuery, ApiResponse<PagedResult<FgsVehicleSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsVehicleSummaryDto>>> Handle(
        ListActiveVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsVehicleListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsVehicleSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsVehicleSummaryDto>>(ex);
        }
    }
}
