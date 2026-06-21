using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.UpdateFgsSetupTaxDetail;

public sealed class UpdateFgsSetupTaxDetailCommandHandler(
    IFgsSetupTaxDetailWriteService writeService,
    ILogger<UpdateFgsSetupTaxDetailCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupTaxDetailCommand, ApiResponse<FgsSetupTaxDetailDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDetailDto>> Handle(
        UpdateFgsSetupTaxDetailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated tax detail {Id}", result.Id);
            return ApiResponse<FgsSetupTaxDetailDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update tax detail {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDetailDto>(ex);
        }
    }
}
