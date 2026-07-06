using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.CreateFgsSetupPaymentTerm;

public sealed class CreateFgsSetupPaymentTermCommandHandler(
    IFgsSetupPaymentTermWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupPaymentTermCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPaymentTermCommand, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        CreateFgsSetupPaymentTermCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created payment term {Id} with code {Name}", result.Id, result.Name);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "paymentterm"),
                cancellationToken);
        return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
