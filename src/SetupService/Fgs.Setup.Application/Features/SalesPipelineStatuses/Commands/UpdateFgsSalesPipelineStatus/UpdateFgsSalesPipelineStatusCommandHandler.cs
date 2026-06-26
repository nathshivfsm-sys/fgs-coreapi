using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.UpdateFgsSalesPipelineStatus;

public sealed class UpdateFgsSalesPipelineStatusCommandHandler(
    IFgsSalesPipelineStatusWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSalesPipelineStatusCommandHandler> logger)
    : IRequestHandler<UpdateFgsSalesPipelineStatusCommand, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        UpdateFgsSalesPipelineStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated sales pipeline status {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "salespipelinestatuses"),
                cancellationToken);
        return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result);
    }
}
