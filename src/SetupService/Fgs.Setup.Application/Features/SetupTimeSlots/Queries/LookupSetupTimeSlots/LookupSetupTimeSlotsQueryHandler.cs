using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.LookupSetupTimeSlots;

public sealed class LookupSetupTimeSlotsQueryHandler(IFgsSetupTimeSlotReadRepository readRepository)
    : IRequestHandler<LookupSetupTimeSlotsQuery, ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>> Handle(
        LookupSetupTimeSlotsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupTimeSlotLookupDto>>(ex);
        }
    }
}
