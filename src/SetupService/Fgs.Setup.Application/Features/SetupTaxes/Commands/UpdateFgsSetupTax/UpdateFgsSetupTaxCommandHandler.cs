using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.UpdateFgsSetupTax;

public sealed class UpdateFgsSetupTaxCommandHandler(
    IFgsSetupTaxWriteService writeService,
    ILogger<UpdateFgsSetupTaxCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupTaxCommand, ApiResponse<FgsSetupTaxDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDto>> Handle(
        UpdateFgsSetupTaxCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated tax {Id}", result.Id);
            return ApiResponse<FgsSetupTaxDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update tax {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDto>(ex);
        }
    }
}
