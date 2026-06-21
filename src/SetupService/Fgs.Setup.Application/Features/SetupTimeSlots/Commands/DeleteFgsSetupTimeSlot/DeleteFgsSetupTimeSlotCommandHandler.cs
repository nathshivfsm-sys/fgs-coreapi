using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.DeleteFgsSetupTimeSlot;

public sealed class DeleteFgsSetupTimeSlotCommandHandler(
    IFgsSetupTimeSlotWriteService writeService,
    ILogger<DeleteFgsSetupTimeSlotCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupTimeSlotCommand, ApiResponse<FgsSetupTimeSlotDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTimeSlotDetailDto>> Handle(
        DeleteFgsSetupTimeSlotCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted time slot {Id}", result.Id);
            return ApiResponse<FgsSetupTimeSlotDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete time slot {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTimeSlotDetailDto>(ex);
        }
    }
}
