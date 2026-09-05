using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.PatchTenant;

public sealed class PatchTenantCommandHandler(
    IUserWriteRepository<FgsTenant> tenantWriteRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<PatchTenantCommand, ApiResponse<TenantDetailDto>>
{
    public async Task<ApiResponse<TenantDetailDto>> Handle(
        PatchTenantCommand request,
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

        ApplyPatch(tenant, request.Dto, DateTimeOffset.UtcNow);
        tenant.UpdatedBy = userContext.UserId?.ToString() ?? userContext.DisplayName;
        tenantWriteRepository.Update(tenant);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant",
                request.TenantId.ToString()),
            cancellationToken);

        return ApiResponse<TenantDetailDto>.Ok(UpdateTenantCommandHandler.Map(tenant));
    }

    private static void ApplyPatch(FgsTenant tenant, TenantPatchDto dto, DateTimeOffset now)
    {
        if (dto.Name is not null)
        {
            tenant.Name = dto.Name.Trim();
        }

        if (dto.LegalName is not null)
        {
            tenant.LegalName = TrimOrNull(dto.LegalName);
        }

        if (dto.Email is not null)
        {
            tenant.Email = TrimOrNull(dto.Email);
        }

        if (dto.PhoneNumber is not null)
        {
            tenant.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber)
                ? null
                : SignupPhoneNormalizer.ToStorageFormat(dto.PhoneNumber);
        }

        if (dto.Website is not null)
        {
            tenant.Website = TrimOrNull(dto.Website);
        }

        if (dto.DefaultCurrency is not null)
        {
            tenant.DefaultCurrency = TrimOrNull(dto.DefaultCurrency);
        }

        if (dto.DefaultLanguageId.HasValue)
        {
            tenant.DefaultLanguageId = dto.DefaultLanguageId;
        }

        if (dto.IsActive.HasValue)
        {
            tenant.IsActive = dto.IsActive.Value;
        }

        tenant.UpdatedOn = now;
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
