using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.PatchFgsSalesPipelineStatus;

public sealed class PatchFgsSalesPipelineStatusCommandHandler(
    IFgsSalesPipelineStatusWriteService writeService,
    ILogger<PatchFgsSalesPipelineStatusCommandHandler> logger)
    : IRequestHandler<PatchFgsSalesPipelineStatusCommand, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        PatchFgsSalesPipelineStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd sales pipeline status {Id}", result.Id);
            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch sales pipeline status {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesPipelineStatusDetailDto>(ex);
        }
    }
}
