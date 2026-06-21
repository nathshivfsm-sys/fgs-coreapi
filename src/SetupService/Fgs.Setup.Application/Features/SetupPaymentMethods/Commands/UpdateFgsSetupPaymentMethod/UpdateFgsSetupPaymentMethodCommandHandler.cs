using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.UpdateFgsSetupPaymentMethod;

public sealed class UpdateFgsSetupPaymentMethodCommandHandler(
    IFgsSetupPaymentMethodWriteService writeService,
    ILogger<UpdateFgsSetupPaymentMethodCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPaymentMethodCommand, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        UpdateFgsSetupPaymentMethodCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated payment method {Id}", result.Id);
            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update payment method {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentMethodDetailDto>(ex);
        }
    }
}
