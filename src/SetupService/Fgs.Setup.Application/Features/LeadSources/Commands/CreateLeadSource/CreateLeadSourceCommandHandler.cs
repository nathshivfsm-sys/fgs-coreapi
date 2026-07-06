using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.CreateLeadSource;

public sealed class CreateLeadSourceCommandHandler(
    ILeadSourceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateLeadSourceCommandHandler> logger)
    : IRequestHandler<CreateLeadSourceCommand, ApiResponse<LeadSourceDetailDto>>
{
    public async Task<ApiResponse<LeadSourceDetailDto>> Handle(
        CreateLeadSourceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created lead source {Id} with code {SourceCode}", result.Id, result.SourceCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "leadsource"),
                cancellationToken);
        return ApiResponse<LeadSourceDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
