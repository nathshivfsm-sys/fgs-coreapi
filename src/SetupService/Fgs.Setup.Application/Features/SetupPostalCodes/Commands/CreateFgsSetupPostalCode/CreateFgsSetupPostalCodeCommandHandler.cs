using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.CreateFgsSetupPostalCode;

public sealed class CreateFgsSetupPostalCodeCommandHandler(
    IFgsSetupPostalCodeWriteService writeService,
    ILogger<CreateFgsSetupPostalCodeCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPostalCodeCommand, ApiResponse<FgsSetupPostalCodeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPostalCodeDetailDto>> Handle(
        CreateFgsSetupPostalCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created postal code {Id} with code {PostalCode}", result.Id, result.PostalCode);
            return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create postal code");
            return CatalogCrudExceptionMapper.MapException<FgsSetupPostalCodeDetailDto>(ex);
        }
    }
}
