using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.CreateFgsSalesPipelineStatus;

public sealed class CreateFgsSalesPipelineStatusCommandHandler(
    IFgsSalesPipelineStatusWriteService writeService,
    ILogger<CreateFgsSalesPipelineStatusCommandHandler> logger)
    : IRequestHandler<CreateFgsSalesPipelineStatusCommand, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        CreateFgsSalesPipelineStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created sales pipeline status {Id} with code {StatusCode}", result.Id, result.StatusCode);
            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create sales pipeline status");
            return CatalogCrudExceptionMapper.MapException<FgsSalesPipelineStatusDetailDto>(ex);
        }
    }
}
