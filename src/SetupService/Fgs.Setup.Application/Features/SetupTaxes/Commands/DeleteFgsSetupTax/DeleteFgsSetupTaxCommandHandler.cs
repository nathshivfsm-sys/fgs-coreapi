using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.DeleteFgsSetupTax;

public sealed class DeleteFgsSetupTaxCommandHandler(
    IFgsSetupTaxWriteService writeService,
    ILogger<DeleteFgsSetupTaxCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupTaxCommand, ApiResponse<FgsSetupTaxDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDto>> Handle(
        DeleteFgsSetupTaxCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted tax {Id}", result.Id);
            return ApiResponse<FgsSetupTaxDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete tax {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDto>(ex);
        }
    }
}
