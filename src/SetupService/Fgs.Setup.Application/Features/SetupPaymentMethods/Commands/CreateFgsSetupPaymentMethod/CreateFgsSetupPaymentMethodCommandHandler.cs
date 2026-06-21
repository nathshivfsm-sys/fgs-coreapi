using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.CreateFgsSetupPaymentMethod;

public sealed class CreateFgsSetupPaymentMethodCommandHandler(
    IFgsSetupPaymentMethodWriteService writeService,
    ILogger<CreateFgsSetupPaymentMethodCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPaymentMethodCommand, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        CreateFgsSetupPaymentMethodCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created payment method {Id} with code {DisplayName}", result.Id, result.DisplayName);
            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create payment method");
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentMethodDetailDto>(ex);
        }
    }
}
