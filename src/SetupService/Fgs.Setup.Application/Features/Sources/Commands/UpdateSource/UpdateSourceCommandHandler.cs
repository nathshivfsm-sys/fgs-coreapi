using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Sources.Commands.UpdateSource;

public sealed class UpdateSourceCommandHandler(
    ISourceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateSourceCommandHandler> logger)
    : IRequestHandler<UpdateSourceCommand, ApiResponse<SourceDetailDto>>
{
    public async Task<ApiResponse<SourceDetailDto>> Handle(
        UpdateSourceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated source {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "source"),
                cancellationToken);
        return ApiResponse<SourceDetailDto>.Ok(result);
    }
}
