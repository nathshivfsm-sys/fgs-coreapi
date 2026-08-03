using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ServiceAccountsSetups;

public sealed class FgsTenantServiceAccountsSetupWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsTenantServiceAccountsSetupWriteService
{
    public async Task<FgsTenantServiceAccountsSetupDetailDto> UpdateAsync(
        FgsTenantServiceAccountsSetupUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindOrCreateCurrentAsync(cancellationToken);
        ApplyUpdate(entity, dto);
        StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsTenantServiceAccountsSetupDetailDto> PatchAsync(
        FgsTenantServiceAccountsSetupPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindOrCreateCurrentAsync(cancellationToken);
        ApplyPatch(entity, dto);
        StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsTenantServiceAccountsSetup> FindOrCreateCurrentAsync(CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await context.FgsTenantServiceAccountsSetups
            .FirstOrDefaultAsync(e => e.TenantId == tenantId && e.CompanyId == companyId, cancellationToken);

        if (entity is not null)
        {
            return entity;
        }

        // Existing companies won't have a row until the first update/patch is applied, so create it on demand.
        entity = new FgsTenantServiceAccountsSetup
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = ResolveActor()
        };
        await context.FgsTenantServiceAccountsSetups.AddAsync(entity, cancellationToken);
        return entity;
    }

    private static void ApplyUpdate(FgsTenantServiceAccountsSetup entity, FgsTenantServiceAccountsSetupUpdateDto dto)
    {
        entity.BankAccountId = dto.BankAccountId;
        entity.AccountsReceivableAccountId = dto.AccountsReceivableAccountId;
        entity.RevenueAccountId = dto.RevenueAccountId;
        entity.DiscountAccountId = dto.DiscountAccountId;
        entity.SalesTaxPayableAccountId = dto.SalesTaxPayableAccountId;
        entity.InventoryAccountId = dto.InventoryAccountId;
        entity.COGSAccountId = dto.COGSAccountId;
        entity.UndepositedFundsAccountId = dto.UndepositedFundsAccountId;
        entity.ProcessingFeeAccountId = dto.ProcessingFeeAccountId;
        entity.AccountsPayableAccountId = dto.AccountsPayableAccountId;
        entity.IsActive = dto.IsActive;
    }

    private static void ApplyPatch(FgsTenantServiceAccountsSetup entity, FgsTenantServiceAccountsSetupPatchDto dto)
    {
        if (dto.BankAccountId.HasValue)
        {
            entity.BankAccountId = dto.BankAccountId;
        }

        if (dto.AccountsReceivableAccountId.HasValue)
        {
            entity.AccountsReceivableAccountId = dto.AccountsReceivableAccountId;
        }

        if (dto.RevenueAccountId.HasValue)
        {
            entity.RevenueAccountId = dto.RevenueAccountId;
        }

        if (dto.DiscountAccountId.HasValue)
        {
            entity.DiscountAccountId = dto.DiscountAccountId;
        }

        if (dto.SalesTaxPayableAccountId.HasValue)
        {
            entity.SalesTaxPayableAccountId = dto.SalesTaxPayableAccountId;
        }

        if (dto.InventoryAccountId.HasValue)
        {
            entity.InventoryAccountId = dto.InventoryAccountId;
        }

        if (dto.COGSAccountId.HasValue)
        {
            entity.COGSAccountId = dto.COGSAccountId;
        }

        if (dto.UndepositedFundsAccountId.HasValue)
        {
            entity.UndepositedFundsAccountId = dto.UndepositedFundsAccountId;
        }

        if (dto.ProcessingFeeAccountId.HasValue)
        {
            entity.ProcessingFeeAccountId = dto.ProcessingFeeAccountId;
        }

        if (dto.AccountsPayableAccountId.HasValue)
        {
            entity.AccountsPayableAccountId = dto.AccountsPayableAccountId;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }
    }

    private void StampForUpdate(FgsTenantServiceAccountsSetup entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() =>
        userContext.UserId?.ToString() ?? "system";

    private static FgsTenantServiceAccountsSetupDetailDto MapToDetail(FgsTenantServiceAccountsSetup entity) =>
        new(
            entity.TenantId,
            entity.CompanyId,
            entity.BankAccountId,
            entity.AccountsReceivableAccountId,
            entity.RevenueAccountId,
            entity.DiscountAccountId,
            entity.SalesTaxPayableAccountId,
            entity.InventoryAccountId,
            entity.COGSAccountId,
            entity.UndepositedFundsAccountId,
            entity.ProcessingFeeAccountId,
            entity.AccountsPayableAccountId,
            entity.IsActive);
}
