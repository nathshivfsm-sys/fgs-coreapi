using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.PatchFgsSetupPaymentMethod;

public sealed class PatchFgsSetupPaymentMethodCommandHandler(
    IFgsSetupPaymentMethodWriteService writeService,
    ILogger<PatchFgsSetupPaymentMethodCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPaymentMethodCommand, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        PatchFgsSetupPaymentMethodCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd payment method {Id}", result.Id);
            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch payment method {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentMethodDetailDto>(ex);
        }
    }
}
