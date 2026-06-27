using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;

public sealed class UpdateTenantStatusCommandHandler(
    IUserWriteRepository<FgsTenant> tenantWriteRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache)
    : IRequestHandler<UpdateTenantStatusCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        UpdateTenantStatusCommand request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantWriteRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<object>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        tenant.FgsTenantStatusId = request.Request.FgsTenantStatusId;
        tenant.UpdatedOn = DateTimeOffset.UtcNow;
        if (request.Request.FgsTenantStatusId == TenantStatusIds.Active)
        {
            tenant.IsActive = true;
        }

        tenantWriteRepository.Update(tenant);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant",
                request.TenantId.ToString()),
            cancellationToken);
        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant-companies",
                "list"),
            cancellationToken);

        return ApiResponse<object>.Ok(new object());
    }
}
