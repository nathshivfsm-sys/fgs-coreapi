using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.ListSetupZones;

public sealed class ListSetupZonesQueryHandler(IFgsSetupZoneReadRepository readRepository)
    : IRequestHandler<ListSetupZonesQuery, ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>> Handle(
        ListSetupZonesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupZoneSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupZoneSummaryDto>>(ex);
        }
    }
}
