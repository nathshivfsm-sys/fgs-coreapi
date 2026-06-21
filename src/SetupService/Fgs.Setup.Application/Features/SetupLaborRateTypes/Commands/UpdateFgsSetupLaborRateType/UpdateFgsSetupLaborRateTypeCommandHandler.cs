using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.UpdateFgsSetupLaborRateType;

public sealed class UpdateFgsSetupLaborRateTypeCommandHandler(
    IFgsSetupLaborRateTypeWriteService writeService,
    ILogger<UpdateFgsSetupLaborRateTypeCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupLaborRateTypeCommand, ApiResponse<FgsSetupLaborRateTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupLaborRateTypeDetailDto>> Handle(
        UpdateFgsSetupLaborRateTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated labor rate type {Id}", result.Id);
            return ApiResponse<FgsSetupLaborRateTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update labor rate type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupLaborRateTypeDetailDto>(ex);
        }
    }
}
