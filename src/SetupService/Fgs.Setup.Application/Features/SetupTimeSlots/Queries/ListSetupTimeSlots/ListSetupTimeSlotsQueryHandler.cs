using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.ListSetupTimeSlots;

public sealed class ListSetupTimeSlotsQueryHandler(IFgsSetupTimeSlotReadRepository readRepository)
    : IRequestHandler<ListSetupTimeSlotsQuery, ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>> Handle(
        ListSetupTimeSlotsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsSetupTimeSlotSummaryDto>>.Ok(result);
    }
}
