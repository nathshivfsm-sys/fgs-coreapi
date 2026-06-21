using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.DeleteFgsSetupLaborRateType;

public sealed class DeleteFgsSetupLaborRateTypeCommandHandler(
    IFgsSetupLaborRateTypeWriteService writeService,
    ILogger<DeleteFgsSetupLaborRateTypeCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupLaborRateTypeCommand, ApiResponse<FgsSetupLaborRateTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupLaborRateTypeDetailDto>> Handle(
        DeleteFgsSetupLaborRateTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted labor rate type {Id}", result.Id);
            return ApiResponse<FgsSetupLaborRateTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete labor rate type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupLaborRateTypeDetailDto>(ex);
        }
    }
}
