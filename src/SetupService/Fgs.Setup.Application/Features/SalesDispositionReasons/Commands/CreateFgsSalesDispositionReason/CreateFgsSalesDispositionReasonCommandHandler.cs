using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.CreateFgsSalesDispositionReason;

public sealed class CreateFgsSalesDispositionReasonCommandHandler(
    IFgsSalesDispositionReasonWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSalesDispositionReasonCommandHandler> logger)
    : IRequestHandler<CreateFgsSalesDispositionReasonCommand, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        CreateFgsSalesDispositionReasonCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created sales disposition reason {Id} with code {DispositionReasonCode}", result.Id, result.DispositionReasonCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "salesdispositionreasons"),
                cancellationToken);
        return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
