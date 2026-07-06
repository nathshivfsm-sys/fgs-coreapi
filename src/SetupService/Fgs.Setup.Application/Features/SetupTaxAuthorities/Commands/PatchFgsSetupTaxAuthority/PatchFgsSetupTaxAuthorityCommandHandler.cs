using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.PatchFgsSetupTaxAuthority;

public sealed class PatchFgsSetupTaxAuthorityCommandHandler(
    IFgsSetupTaxAuthorityWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupTaxAuthorityCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupTaxAuthorityCommand, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        PatchFgsSetupTaxAuthorityCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd tax authority {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "taxauthority"),
                cancellationToken);
        return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
    }
}
