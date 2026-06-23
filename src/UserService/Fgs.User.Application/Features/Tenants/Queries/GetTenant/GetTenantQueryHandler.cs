using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenant;

public sealed class GetTenantQueryHandler(IUserReadRepository<FgsTenant> tenantReadRepository)
    : IRequestHandler<GetTenantQuery, ApiResponse<TenantDto>>
{
    public async Task<ApiResponse<TenantDto>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await tenantReadRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<TenantDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        return ApiResponse<TenantDto>.Ok(new TenantDto(
            tenant.Id,
            tenant.TenantCode,
            tenant.Name,
            tenant.FgsTenantStatusId,
            tenant.StorageBucketName));
    }
}
