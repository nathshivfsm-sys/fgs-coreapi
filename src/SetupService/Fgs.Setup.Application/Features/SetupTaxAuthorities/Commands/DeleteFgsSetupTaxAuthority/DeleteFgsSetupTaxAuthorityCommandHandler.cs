using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.DeleteFgsSetupTaxAuthority;

public sealed class DeleteFgsSetupTaxAuthorityCommandHandler(
    IFgsSetupTaxAuthorityWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsSetupTaxAuthorityCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupTaxAuthorityCommand, ApiResponse<FgsSetupTaxAuthorityDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxAuthorityDetailDto>> Handle(
        DeleteFgsSetupTaxAuthorityCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted tax authority {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "taxauthorities"),
                cancellationToken);
            return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<FgsSetupTaxAuthorityDetailDto>.Fail([ex.Message], ApiStatusCodes.Conflict);
        }
    }
}
