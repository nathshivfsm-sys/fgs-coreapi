using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.CreateFgsSetupTimeSlot;

public sealed class CreateFgsSetupTimeSlotCommandHandler(
    IFgsSetupTimeSlotWriteService writeService,
    ILogger<CreateFgsSetupTimeSlotCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupTimeSlotCommand, ApiResponse<FgsSetupTimeSlotDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTimeSlotDetailDto>> Handle(
        CreateFgsSetupTimeSlotCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created time slot {Id} with code {Code}", result.Id, result.Code);
            return ApiResponse<FgsSetupTimeSlotDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create time slot");
            return CatalogCrudExceptionMapper.MapException<FgsSetupTimeSlotDetailDto>(ex);
        }
    }
}
