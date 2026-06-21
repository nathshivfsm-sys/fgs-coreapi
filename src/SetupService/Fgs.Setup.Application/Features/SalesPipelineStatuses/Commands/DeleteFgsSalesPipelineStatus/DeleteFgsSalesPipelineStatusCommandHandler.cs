using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.DeleteFgsSalesPipelineStatus;

public sealed class DeleteFgsSalesPipelineStatusCommandHandler(
    IFgsSalesPipelineStatusWriteService writeService,
    ILogger<DeleteFgsSalesPipelineStatusCommandHandler> logger)
    : IRequestHandler<DeleteFgsSalesPipelineStatusCommand, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        DeleteFgsSalesPipelineStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted sales pipeline status {Id}", result.Id);
            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete sales pipeline status {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSalesPipelineStatusDetailDto>(ex);
        }
    }
}
