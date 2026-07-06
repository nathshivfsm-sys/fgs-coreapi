using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.DeleteLeadSource;

public sealed class DeleteLeadSourceCommandHandler(
    ILeadSourceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteLeadSourceCommandHandler> logger)
    : IRequestHandler<DeleteLeadSourceCommand, ApiResponse<LeadSourceDetailDto>>
{
    public async Task<ApiResponse<LeadSourceDetailDto>> Handle(
        DeleteLeadSourceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted lead source {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "leadsource"),
                cancellationToken);
        return ApiResponse<LeadSourceDetailDto>.Ok(result);
    }
}
