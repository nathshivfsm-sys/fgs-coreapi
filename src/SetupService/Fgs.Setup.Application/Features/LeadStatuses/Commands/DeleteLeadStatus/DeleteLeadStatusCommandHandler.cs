using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.DeleteLeadStatus;

public sealed class DeleteLeadStatusCommandHandler(
    ILeadStatusWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteLeadStatusCommandHandler> logger)
    : IRequestHandler<DeleteLeadStatusCommand, ApiResponse<LeadStatusDetailDto>>
{
    public async Task<ApiResponse<LeadStatusDetailDto>> Handle(
        DeleteLeadStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted lead status {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "leadstatus"),
                cancellationToken);
        return ApiResponse<LeadStatusDetailDto>.Ok(result);
    }
}
