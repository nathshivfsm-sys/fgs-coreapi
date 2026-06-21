using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.UpdateFgsSalesActivityType;

public sealed class UpdateFgsSalesActivityTypeCommandHandler(
    IFgsSalesActivityTypeWriteService writeService,
    ILogger<UpdateFgsSalesActivityTypeCommandHandler> logger)
    : IRequestHandler<UpdateFgsSalesActivityTypeCommand, ApiResponse<FgsSalesActivityTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityTypeDetailDto>> Handle(
        UpdateFgsSalesActivityTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated sales activity type {Id}", result.Id);
            return ApiResponse<FgsSalesActivityTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update sales activity type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityTypeDetailDto>(ex);
        }
    }
}
