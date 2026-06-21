using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.PatchFgsSetupTax;

public sealed class PatchFgsSetupTaxCommandHandler(
    IFgsSetupTaxWriteService writeService,
    ILogger<PatchFgsSetupTaxCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupTaxCommand, ApiResponse<FgsSetupTaxDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDto>> Handle(
        PatchFgsSetupTaxCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd tax {Id}", result.Id);
            return ApiResponse<FgsSetupTaxDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch tax {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDto>(ex);
        }
    }
}
