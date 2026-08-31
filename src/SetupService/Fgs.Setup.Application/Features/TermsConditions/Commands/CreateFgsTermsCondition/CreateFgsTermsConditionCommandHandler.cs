using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TermsConditions.Commands.CreateFgsTermsCondition;

public sealed class CreateFgsTermsConditionCommandHandler(
    IFgsTermsConditionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsTermsConditionCommandHandler> logger)
    : IRequestHandler<CreateFgsTermsConditionCommand, ApiResponse<FgsTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsTermsConditionDetailDto>> Handle(
        CreateFgsTermsConditionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created terms condition {Id} with code {Code} version {Version}",
            result.Id,
            result.Code,
            result.VersionNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "termscondition"),
            cancellationToken);
        return ApiResponse<FgsTermsConditionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
