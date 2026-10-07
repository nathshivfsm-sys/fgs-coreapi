using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Sources.Commands.DeleteSource;

public sealed class DeleteSourceCommandHandler(
    ISourceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteSourceCommandHandler> logger)
    : IRequestHandler<DeleteSourceCommand, ApiResponse<SourceDetailDto>>
{
    public async Task<ApiResponse<SourceDetailDto>> Handle(
        DeleteSourceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted source {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "source"),
                cancellationToken);
        return ApiResponse<SourceDetailDto>.Ok(result);
    }
}
