using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.UpdateFgsSetupPaymentTerm;

public sealed class UpdateFgsSetupPaymentTermCommandHandler(
    IFgsSetupPaymentTermWriteService writeService,
    ILogger<UpdateFgsSetupPaymentTermCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPaymentTermCommand, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        UpdateFgsSetupPaymentTermCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated payment term {Id}", result.Id);
            return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update payment term {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPaymentTermDetailDto>(ex);
        }
    }
}
