using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.UpdateFgsSalesDispositionReason;

public sealed class UpdateFgsSalesDispositionReasonCommandHandler(
    IFgsSalesDispositionReasonWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSalesDispositionReasonCommandHandler> logger)
    : IRequestHandler<UpdateFgsSalesDispositionReasonCommand, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        UpdateFgsSalesDispositionReasonCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated sales disposition reason {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "salesdispositionreason"),
                cancellationToken);
        return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result);
    }
}
