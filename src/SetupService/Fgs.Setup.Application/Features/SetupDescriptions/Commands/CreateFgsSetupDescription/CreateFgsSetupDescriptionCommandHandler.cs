using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.CreateFgsSetupDescription;

public sealed class CreateFgsSetupDescriptionCommandHandler(
    IFgsSetupDescriptionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupDescriptionCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupDescriptionCommand, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        CreateFgsSetupDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created setup description {Id} with code {DescriptionTypeCode}", result.Id, result.DescriptionTypeCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "setupdescription"),
                cancellationToken);
        return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
