using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.CreateFgsSetupPaymentMethod;

public sealed class CreateFgsSetupPaymentMethodCommandHandler(
    IFgsSetupPaymentMethodWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupPaymentMethodCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPaymentMethodCommand, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        CreateFgsSetupPaymentMethodCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created payment method {Id} with code {DisplayName}", result.Id, result.DisplayName);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "paymentmethods"),
                cancellationToken);
        return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
