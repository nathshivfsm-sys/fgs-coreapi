using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.UpdateFgsSetupPostalCode;

public sealed class UpdateFgsSetupPostalCodeCommandHandler(
    IFgsSetupPostalCodeWriteService writeService,
    ILogger<UpdateFgsSetupPostalCodeCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupPostalCodeCommand, ApiResponse<FgsSetupPostalCodeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPostalCodeDetailDto>> Handle(
        UpdateFgsSetupPostalCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated postal code {Id}", result.Id);
            return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update postal code {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupPostalCodeDetailDto>(ex);
        }
    }
}
