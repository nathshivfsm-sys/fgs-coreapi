using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ServiceSetups;

public sealed class FgsTenantServiceSetupWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsTenantServiceSetupWriteService
{
    public async Task<FgsTenantServiceSetupDetailDto> UpdateAsync(
        FgsTenantServiceSetupUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindCurrentAsync(cancellationToken)
            ?? throw new KeyNotFoundException("Service setup was not found for the current company.");

        ApplyUpdate(entity, dto);
        StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsTenantServiceSetupDetailDto> PatchAsync(
        FgsTenantServiceSetupPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindCurrentAsync(cancellationToken)
            ?? throw new KeyNotFoundException("Service setup was not found for the current company.");

        ApplyPatch(entity, dto);
        StampForUpdate(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsTenantServiceSetup?> FindCurrentAsync(CancellationToken cancellationToken)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return await context.FgsTenantServiceSetups
            .FirstOrDefaultAsync(e => e.TenantId == tenantId && e.CompanyId == companyId, cancellationToken);
    }

    private static void ApplyUpdate(FgsTenantServiceSetup entity, FgsTenantServiceSetupUpdateDto dto)
    {
        entity.TimeCardOptionId = dto.TimeCardOptionId;
        entity.AccountingIntegrationTypeId = dto.AccountingIntegrationTypeId;
        entity.UseExternalTaxCalculationProvider = dto.UseExternalTaxCalculationProvider;
        entity.EnableCallBookingWidget = dto.EnableCallBookingWidget;
        entity.EnablePaymentWidget = dto.EnablePaymentWidget;
        entity.EnableCustomerPortal = dto.EnableCustomerPortal;
        entity.EnableRulesManagement = dto.EnableRulesManagement;
        entity.EnableAutoArrive = dto.EnableAutoArrive;
        entity.WorkLocationRadiusForAutoArrive = dto.WorkLocationRadiusForAutoArrive;
        entity.OTStartTime = dto.OTStartTime;
        entity.OTEndTime = dto.OTEndTime;
        entity.DTStartTime = dto.DTStartTime;
        entity.DTEndTime = dto.DTEndTime;
        entity.BillHoursFromDispatchOrArrive = NormalizeBillHours(dto.BillHoursFromDispatchOrArrive);
        entity.SourceCodeRequiredOnWorkOrder = dto.SourceCodeRequiredOnWorkOrder;
        entity.SourceCodeRequiredOnServiceLocation = dto.SourceCodeRequiredOnServiceLocation;
        entity.BillToStartNumber = dto.BillToStartNumber;
        entity.POStartNumber = dto.POStartNumber;
        entity.QuoteStartNumber = dto.QuoteStartNumber;
        entity.WorkOrderStartNumber = dto.WorkOrderStartNumber;
        entity.InvoiceNumberPrefix = TrimOrNull(dto.InvoiceNumberPrefix);
        entity.QuoteNumberPrefix = TrimOrNull(dto.QuoteNumberPrefix);
        entity.PONumberPrefix = TrimOrNull(dto.PONumberPrefix);
        entity.WorkOrderNumberPrefix = TrimOrNull(dto.WorkOrderNumberPrefix);
        entity.InvoiceBatchNumberFormat = TrimOrNull(dto.InvoiceBatchNumberFormat);
        entity.EstimateRevisionCreationMode = NormalizeEstimateRevisionCreationMode(dto.EstimateRevisionCreationMode);
        entity.IsActive = dto.IsActive;
    }

    private static void ApplyPatch(FgsTenantServiceSetup entity, FgsTenantServiceSetupPatchDto dto)
    {
        if (dto.TimeCardOptionId.HasValue)
        {
            entity.TimeCardOptionId = dto.TimeCardOptionId.Value;
        }

        if (dto.AccountingIntegrationTypeId.HasValue)
        {
            entity.AccountingIntegrationTypeId = dto.AccountingIntegrationTypeId;
        }

        if (dto.UseExternalTaxCalculationProvider.HasValue)
        {
            entity.UseExternalTaxCalculationProvider = dto.UseExternalTaxCalculationProvider.Value;
        }

        if (dto.EnableCallBookingWidget.HasValue)
        {
            entity.EnableCallBookingWidget = dto.EnableCallBookingWidget.Value;
        }

        if (dto.EnablePaymentWidget.HasValue)
        {
            entity.EnablePaymentWidget = dto.EnablePaymentWidget.Value;
        }

        if (dto.EnableCustomerPortal.HasValue)
        {
            entity.EnableCustomerPortal = dto.EnableCustomerPortal.Value;
        }

        if (dto.EnableRulesManagement.HasValue)
        {
            entity.EnableRulesManagement = dto.EnableRulesManagement.Value;
        }

        if (dto.EnableAutoArrive.HasValue)
        {
            entity.EnableAutoArrive = dto.EnableAutoArrive.Value;
        }

        if (dto.WorkLocationRadiusForAutoArrive.HasValue)
        {
            entity.WorkLocationRadiusForAutoArrive = dto.WorkLocationRadiusForAutoArrive;
        }

        if (dto.OTStartTime.HasValue)
        {
            entity.OTStartTime = dto.OTStartTime;
        }

        if (dto.OTEndTime.HasValue)
        {
            entity.OTEndTime = dto.OTEndTime;
        }

        if (dto.DTStartTime.HasValue)
        {
            entity.DTStartTime = dto.DTStartTime;
        }

        if (dto.DTEndTime.HasValue)
        {
            entity.DTEndTime = dto.DTEndTime;
        }

        if (dto.BillHoursFromDispatchOrArrive is not null)
        {
            entity.BillHoursFromDispatchOrArrive = NormalizeBillHours(dto.BillHoursFromDispatchOrArrive);
        }

        if (dto.SourceCodeRequiredOnWorkOrder.HasValue)
        {
            entity.SourceCodeRequiredOnWorkOrder = dto.SourceCodeRequiredOnWorkOrder.Value;
        }

        if (dto.SourceCodeRequiredOnServiceLocation.HasValue)
        {
            entity.SourceCodeRequiredOnServiceLocation = dto.SourceCodeRequiredOnServiceLocation.Value;
        }

        if (dto.BillToStartNumber.HasValue)
        {
            entity.BillToStartNumber = dto.BillToStartNumber.Value;
        }

        if (dto.POStartNumber.HasValue)
        {
            entity.POStartNumber = dto.POStartNumber.Value;
        }

        if (dto.QuoteStartNumber.HasValue)
        {
            entity.QuoteStartNumber = dto.QuoteStartNumber.Value;
        }

        if (dto.WorkOrderStartNumber.HasValue)
        {
            entity.WorkOrderStartNumber = dto.WorkOrderStartNumber.Value;
        }

        if (dto.InvoiceNumberPrefix is not null)
        {
            entity.InvoiceNumberPrefix = TrimOrNull(dto.InvoiceNumberPrefix);
        }

        if (dto.QuoteNumberPrefix is not null)
        {
            entity.QuoteNumberPrefix = TrimOrNull(dto.QuoteNumberPrefix);
        }

        if (dto.PONumberPrefix is not null)
        {
            entity.PONumberPrefix = TrimOrNull(dto.PONumberPrefix);
        }

        if (dto.WorkOrderNumberPrefix is not null)
        {
            entity.WorkOrderNumberPrefix = TrimOrNull(dto.WorkOrderNumberPrefix);
        }

        if (dto.InvoiceBatchNumberFormat is not null)
        {
            entity.InvoiceBatchNumberFormat = TrimOrNull(dto.InvoiceBatchNumberFormat);
        }

        if (dto.EstimateRevisionCreationMode is not null)
        {
            entity.EstimateRevisionCreationMode =
                NormalizeEstimateRevisionCreationMode(dto.EstimateRevisionCreationMode);
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }
    }

    private void StampForUpdate(FgsTenantServiceSetup entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string ResolveActor() =>
        userContext.UserId?.ToString() ?? "system";

    private static string NormalizeBillHours(string value) => value.Trim().ToUpperInvariant();

    private static string NormalizeEstimateRevisionCreationMode(string value) => value.Trim();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsTenantServiceSetupDetailDto MapToDetail(FgsTenantServiceSetup entity) =>
        new(
            entity.TenantId,
            entity.CompanyId,
            entity.TimeCardOptionId,
            entity.AccountingIntegrationTypeId,
            entity.UseExternalTaxCalculationProvider,
            entity.EnableCallBookingWidget,
            entity.EnablePaymentWidget,
            entity.EnableCustomerPortal,
            entity.EnableRulesManagement,
            entity.EnableAutoArrive,
            entity.WorkLocationRadiusForAutoArrive,
            entity.OTStartTime,
            entity.OTEndTime,
            entity.DTStartTime,
            entity.DTEndTime,
            entity.BillHoursFromDispatchOrArrive,
            entity.SourceCodeRequiredOnWorkOrder,
            entity.SourceCodeRequiredOnServiceLocation,
            entity.BillToStartNumber,
            entity.POStartNumber,
            entity.QuoteStartNumber,
            entity.WorkOrderStartNumber,
            entity.InvoiceNumberPrefix,
            entity.QuoteNumberPrefix,
            entity.PONumberPrefix,
            entity.WorkOrderNumberPrefix,
            entity.InvoiceBatchNumberFormat,
            entity.EstimateRevisionCreationMode,
            entity.IsActive);
}
