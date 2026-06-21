using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.GetFgsSetupTimeSlotById;

public sealed class GetFgsSetupTimeSlotByIdQueryHandler(IFgsSetupTimeSlotReadRepository readRepository)
    : IRequestHandler<GetFgsSetupTimeSlotByIdQuery, ApiResponse<FgsSetupTimeSlotDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTimeSlotDetailDto>> Handle(
        GetFgsSetupTimeSlotByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSetupTimeSlotDetailDto>.Fail(
                    [$"Time Slot '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupTimeSlotDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupTimeSlotDetailDto>(ex);
        }
    }
}
