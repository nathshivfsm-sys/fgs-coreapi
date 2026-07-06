using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.UpdateFgsSetupTaxAuthority;

public sealed class UpdateFgsSetupTaxAuthorityCommandHandler(
    IFgsSetupTaxAuthorityWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsSetupTaxAuthorityCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupTaxAuthorityCommand, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        UpdateFgsSetupTaxAuthorityCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated tax authority {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "taxauthority"),
                cancellationToken);
        return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
    }
}
