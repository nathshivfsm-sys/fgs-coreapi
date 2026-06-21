using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.CreateFgsSetupTaxDetail;

public sealed class CreateFgsSetupTaxDetailCommandHandler(
    IFgsSetupTaxDetailWriteService writeService,
    ILogger<CreateFgsSetupTaxDetailCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupTaxDetailCommand, ApiResponse<FgsSetupTaxDetailDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDetailDto>> Handle(
        CreateFgsSetupTaxDetailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created tax detail {Id} with code {FgsSetupTaxId}", result.Id, result.FgsSetupTaxId);
            return ApiResponse<FgsSetupTaxDetailDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create tax detail");
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDetailDto>(ex);
        }
    }
}
