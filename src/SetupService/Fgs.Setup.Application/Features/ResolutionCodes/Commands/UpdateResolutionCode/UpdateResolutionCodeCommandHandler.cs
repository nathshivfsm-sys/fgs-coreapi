using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.UpdateResolutionCode;

public sealed class UpdateResolutionCodeCommandHandler(
    IResolutionCodeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateResolutionCodeCommandHandler> logger)
    : IRequestHandler<UpdateResolutionCodeCommand, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        UpdateResolutionCodeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated resolution code {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "resolutioncode"),
                cancellationToken);
        return ApiResponse<ResolutionCodeDetailDto>.Ok(result);
    }
}
