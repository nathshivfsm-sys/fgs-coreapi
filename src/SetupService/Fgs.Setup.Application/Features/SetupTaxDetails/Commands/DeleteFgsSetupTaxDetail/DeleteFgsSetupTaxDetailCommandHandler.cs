using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.DeleteFgsSetupTaxDetail;

public sealed class DeleteFgsSetupTaxDetailCommandHandler(
    IFgsSetupTaxDetailWriteService writeService,
    ILogger<DeleteFgsSetupTaxDetailCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupTaxDetailCommand, ApiResponse<FgsSetupTaxDetailDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDetailDto>> Handle(
        DeleteFgsSetupTaxDetailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted tax detail {Id}", result.Id);
            return ApiResponse<FgsSetupTaxDetailDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete tax detail {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDetailDto>(ex);
        }
    }
}
