using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStorageBucket;

public sealed class UpdateTenantStorageBucketCommandHandler(
    IUserWriteRepository<FgsTenant> tenantWriteRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache)
    : IRequestHandler<UpdateTenantStorageBucketCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        UpdateTenantStorageBucketCommand request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantWriteRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<object>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        tenant.StorageBucketName = request.Request.StorageBucketName;
        tenant.UpdatedOn = DateTimeOffset.UtcNow;
        tenantWriteRepository.Update(tenant);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant",
                request.TenantId.ToString()),
            cancellationToken);

        return ApiResponse<object>.Ok(new object());
    }
}
