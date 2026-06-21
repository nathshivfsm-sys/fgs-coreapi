using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.CreateFgsSetupTax;

public sealed class CreateFgsSetupTaxCommandHandler(
    IFgsSetupTaxWriteService writeService,
    ILogger<CreateFgsSetupTaxCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupTaxCommand, ApiResponse<FgsSetupTaxDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDto>> Handle(
        CreateFgsSetupTaxCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created tax {Id} with code {TaxCode}", result.Id, result.TaxCode);
            return ApiResponse<FgsSetupTaxDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create tax");
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDto>(ex);
        }
    }
}
