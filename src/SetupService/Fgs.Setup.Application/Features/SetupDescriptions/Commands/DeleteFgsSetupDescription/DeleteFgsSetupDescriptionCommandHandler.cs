using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.DeleteFgsSetupDescription;

public sealed class DeleteFgsSetupDescriptionCommandHandler(
    IFgsSetupDescriptionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsSetupDescriptionCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupDescriptionCommand, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        DeleteFgsSetupDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted setup description {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "setupdescription"),
                cancellationToken);
        return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result);
    }
}
