using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.CreateLeadDisqualificationReason;

public sealed class CreateLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<CreateLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        CreateLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created lead disqualification reason {Id} with code {ReasonCode}", result.Id, result.ReasonCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "leaddisqualificationreason"),
                cancellationToken);
        return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
