using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.DeleteFgsSetupPaymentMethod;

public sealed class DeleteFgsSetupPaymentMethodCommandHandler(
    IFgsSetupPaymentMethodWriteService writeService,
    ILogger<DeleteFgsSetupPaymentMethodCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupPaymentMethodCommand, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        DeleteFgsSetupPaymentMethodCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted payment method {Id}", result.Id);
            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete payment method {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentMethodDetailDto>(ex);
        }
    }
}
