using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Sources.Commands.PatchSource;

public sealed class PatchSourceCommandHandler(
    ISourceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchSourceCommandHandler> logger)
    : IRequestHandler<PatchSourceCommand, ApiResponse<SourceDetailDto>>
{
    public async Task<ApiResponse<SourceDetailDto>> Handle(
        PatchSourceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched source {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "source"),
                cancellationToken);
        return ApiResponse<SourceDetailDto>.Ok(result);
    }
}
