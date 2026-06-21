using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.ListActiveSetupTimeSlots;

public sealed class ListActiveSetupTimeSlotsQueryHandler(IFgsSetupTimeSlotReadRepository readRepository)
    : IRequestHandler<ListActiveSetupTimeSlotsQuery, ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>> Handle(
        ListActiveSetupTimeSlotsQuery request,
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
                request.Filters ?? new FgsSetupTimeSlotListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupTimeSlotSummaryDto>>(ex);
        }
    }
}
