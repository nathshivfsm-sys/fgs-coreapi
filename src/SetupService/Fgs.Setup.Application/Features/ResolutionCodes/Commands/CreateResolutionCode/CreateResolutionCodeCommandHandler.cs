using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.CreateResolutionCode;

public sealed class CreateResolutionCodeCommandHandler(
    IResolutionCodeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateResolutionCodeCommandHandler> logger)
    : IRequestHandler<CreateResolutionCodeCommand, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        CreateResolutionCodeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created resolution code {Id} with code {ResolutionCode}", result.Id, result.ResolutionCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "resolutioncode"),
                cancellationToken);
        return ApiResponse<ResolutionCodeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
