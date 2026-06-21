using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Commands.PatchFgsSalesActivityType;

public sealed class PatchFgsSalesActivityTypeCommandHandler(
    IFgsSalesActivityTypeWriteService writeService,
    ILogger<PatchFgsSalesActivityTypeCommandHandler> logger)
    : IRequestHandler<PatchFgsSalesActivityTypeCommand, ApiResponse<FgsSalesActivityTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityTypeDetailDto>> Handle(
        PatchFgsSalesActivityTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd sales activity type {Id}", result.Id);
            return ApiResponse<FgsSalesActivityTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch sales activity type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesActivityTypeDetailDto>(ex);
        }
    }
}
