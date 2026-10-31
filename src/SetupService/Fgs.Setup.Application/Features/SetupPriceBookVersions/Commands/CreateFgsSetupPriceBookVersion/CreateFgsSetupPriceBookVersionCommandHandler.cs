using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.CreateFgsSetupPriceBookVersion;

public sealed class CreateFgsSetupPriceBookVersionCommandHandler(
    IFgsSetupPriceBookVersionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupPriceBookVersionCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPriceBookVersionCommand, ApiResponse<FgsSetupPriceBookVersionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPriceBookVersionDetailDto>> Handle(
        CreateFgsSetupPriceBookVersionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created price book version {Id} ({Version})",
            result.Id,
            result.Version);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebookversion"),
            cancellationToken);
        return ApiResponse<FgsSetupPriceBookVersionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
