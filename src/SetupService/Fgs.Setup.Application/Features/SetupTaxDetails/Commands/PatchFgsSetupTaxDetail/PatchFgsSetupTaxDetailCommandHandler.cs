using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.PatchFgsSetupTaxDetail;

public sealed class PatchFgsSetupTaxDetailCommandHandler(
    IFgsSetupTaxDetailWriteService writeService,
    ILogger<PatchFgsSetupTaxDetailCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupTaxDetailCommand, ApiResponse<FgsSetupTaxDetailDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDetailDto>> Handle(
        PatchFgsSetupTaxDetailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd tax detail {Id}", result.Id);
            return ApiResponse<FgsSetupTaxDetailDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch tax detail {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDetailDto>(ex);
        }
    }
}
