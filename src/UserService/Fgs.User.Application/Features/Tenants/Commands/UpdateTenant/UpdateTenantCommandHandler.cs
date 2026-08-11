using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;

public sealed class UpdateTenantCommandHandler(
    IUserWriteRepository<FgsTenant> tenantWriteRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<UpdateTenantCommand, ApiResponse<TenantDetailDto>>
{
    public async Task<ApiResponse<TenantDetailDto>> Handle(
        UpdateTenantCommand request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantAccess<TenantDetailDto>(
            userContext,
            request.TenantId);
        if (denied is not null)
        {
            return denied;
        }

        var tenant = await tenantWriteRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return ApiResponse<TenantDetailDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        ApplyUpdate(tenant, request.Dto, DateTimeOffset.UtcNow);
        tenant.UpdatedBy = userContext.UserId?.ToString() ?? userContext.DisplayName;
        tenantWriteRepository.Update(tenant);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await InvalidateCacheAsync(request.TenantId, cancellationToken);
        return ApiResponse<TenantDetailDto>.Ok(Map(tenant));
    }

    internal static void ApplyUpdate(FgsTenant tenant, TenantUpdateDto dto, DateTimeOffset now)
    {
        tenant.Name = dto.Name.Trim();
        tenant.LegalName = TrimOrNull(dto.LegalName);
        tenant.Email = TrimOrNull(dto.Email);
        tenant.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber)
            ? null
            : SignupPhoneNormalizer.ToStorageFormat(dto.PhoneNumber);
        tenant.Website = TrimOrNull(dto.Website);
        tenant.DefaultCurrency = TrimOrNull(dto.DefaultCurrency);
        tenant.DefaultLanguageId = dto.DefaultLanguageId;
        tenant.IsActive = dto.IsActive;
        tenant.UpdatedOn = now;
    }

    internal static TenantDetailDto Map(FgsTenant tenant) =>
        new(
            tenant.Id,
            tenant.TenantGuid,
            tenant.TenantCode,
            tenant.Name,
            tenant.LegalName,
            tenant.Email,
            tenant.PhoneNumber,
            tenant.Website,
            tenant.DefaultCurrency,
            tenant.DefaultLanguageId,
            tenant.FgsTenantStatusId,
            tenant.StorageBucketName,
            tenant.IsActive);

    private async Task InvalidateCacheAsync(long tenantId, CancellationToken cancellationToken)
    {
        await cache.RemoveAsync(
            CacheKeys.Build(
                tenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant",
                tenantId.ToString()),
            cancellationToken);
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
