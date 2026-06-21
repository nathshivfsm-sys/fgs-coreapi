using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.UpdateFgsSalesPipelineStatus;

public sealed class UpdateFgsSalesPipelineStatusCommandHandler(
    IFgsSalesPipelineStatusWriteService writeService,
    ILogger<UpdateFgsSalesPipelineStatusCommandHandler> logger)
    : IRequestHandler<UpdateFgsSalesPipelineStatusCommand, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        UpdateFgsSalesPipelineStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated sales pipeline status {Id}", result.Id);
            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update sales pipeline status {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesPipelineStatusDetailDto>(ex);
        }
    }
}
