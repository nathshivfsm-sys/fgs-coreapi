using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.PatchFgsSetupTimeSlot;

public sealed class PatchFgsSetupTimeSlotCommandHandler(
    IFgsSetupTimeSlotWriteService writeService,
    ILogger<PatchFgsSetupTimeSlotCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupTimeSlotCommand, ApiResponse<FgsSetupTimeSlotDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTimeSlotDetailDto>> Handle(
        PatchFgsSetupTimeSlotCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd time slot {Id}", result.Id);
            return ApiResponse<FgsSetupTimeSlotDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch time slot {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTimeSlotDetailDto>(ex);
        }
    }
}
