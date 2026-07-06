using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.CreateFgsSetupLaborRateType;

public sealed class CreateFgsSetupLaborRateTypeCommandHandler(
    IFgsSetupLaborRateTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupLaborRateTypeCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupLaborRateTypeCommand, ApiResponse<FgsSetupLaborRateTypeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupLaborRateTypeDetailDto>> Handle(
        CreateFgsSetupLaborRateTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created labor rate type {Id} with code {Name}", result.Id, result.Name);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "laborratetype"),
                cancellationToken);
        return ApiResponse<FgsSetupLaborRateTypeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
