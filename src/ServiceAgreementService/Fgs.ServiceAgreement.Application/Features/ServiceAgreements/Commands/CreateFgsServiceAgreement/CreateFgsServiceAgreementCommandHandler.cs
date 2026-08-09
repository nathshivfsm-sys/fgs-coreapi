using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Commands.CreateFgsServiceAgreement;

public sealed class CreateFgsServiceAgreementCommandHandler(
    IFgsServiceAgreementWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsServiceAgreementCommandHandler> logger)
    : IRequestHandler<CreateFgsServiceAgreementCommand, ApiResponse<FgsServiceAgreementDetailDto>>
{
    public async Task<ApiResponse<FgsServiceAgreementDetailDto>> Handle(
        CreateFgsServiceAgreementCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created service agreement {Id} with number {AgreementNumber}",
            result.Id,
            result.AgreementNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "serviceagreement"),
            cancellationToken);
        return ApiResponse<FgsServiceAgreementDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
