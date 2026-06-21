using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.CreateFgsSalesActivityType;

public sealed class CreateFgsSalesActivityTypeCommandHandler(
    IFgsSalesActivityTypeWriteService writeService,
    ILogger<CreateFgsSalesActivityTypeCommandHandler> logger)
    : IRequestHandler<CreateFgsSalesActivityTypeCommand, ApiResponse<FgsSalesActivityTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityTypeDetailDto>> Handle(
        CreateFgsSalesActivityTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created sales activity type {Id} with code {ActivityTypeCode}", result.Id, result.ActivityTypeCode);
            return ApiResponse<FgsSalesActivityTypeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create sales activity type");
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityTypeDetailDto>(ex);
        }
    }
}
