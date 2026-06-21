using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.ListActiveSetupZones;

public sealed class ListActiveSetupZonesQueryHandler(IFgsSetupZoneReadRepository readRepository)
    : IRequestHandler<ListActiveSetupZonesQuery, ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>> Handle(
        ListActiveSetupZonesQuery request,
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
                request.Filters ?? new FgsSetupZoneListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupZoneSummaryDto>>(ex);
        }
    }
}
