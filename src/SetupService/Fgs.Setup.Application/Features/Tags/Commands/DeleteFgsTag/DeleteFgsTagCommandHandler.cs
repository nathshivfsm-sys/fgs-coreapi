using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Tags.Commands.DeleteFgsTag;

public sealed class DeleteFgsTagCommandHandler(
    IFgsTagWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsTagCommandHandler> logger)
    : IRequestHandler<DeleteFgsTagCommand, ApiResponse<FgsTagDetailDto>>
{
    public async Task<ApiResponse<FgsTagDetailDto>> Handle(
        DeleteFgsTagCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted tag {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "tag"),
                cancellationToken);
        return ApiResponse<FgsTagDetailDto>.Ok(result);
    }
}
