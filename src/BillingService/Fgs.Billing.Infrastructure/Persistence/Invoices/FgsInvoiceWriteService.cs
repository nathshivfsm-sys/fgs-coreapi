using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Common;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Billing.Infrastructure.Persistence.Invoices;

public sealed class FgsInvoiceWriteService : IFgsInvoiceWriteService
{
    private readonly FgsBillingDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BillingEntityAuditHelper _auditHelper;

    public FgsInvoiceWriteService(
        FgsBillingDbContext context,
        IUnitOfWork unitOfWork,
        BillingEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInvoiceDetailDto> CreateAsync(
        FgsInvoiceCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsInvoice
        {
            InvoiceNumber = NormalizeInvoiceNumber(dto.InvoiceNumber),
            InvoiceTypeId = dto.InvoiceTypeId,
            CustomerId = dto.CustomerId,
            ServiceLocationId = dto.ServiceLocationId,
            WorkOrderId = dto.WorkOrderId,
            ProjectId = dto.ProjectId,
            ServiceAgreementId = dto.ServiceAgreementId,
            MaintenanceVisitId = dto.MaintenanceVisitId,
            ServiceJobNum = TrimOrNull(dto.ServiceJobNum),
            IsAgreementBilling = dto.IsAgreementBilling,
            IsRecurringInvoice = dto.IsRecurringInvoice,
            RecurringScheduleId = dto.RecurringScheduleId,
            WorkOrderNumber = TrimOrNull(dto.WorkOrderNumber),
            JobTypeId = dto.JobTypeId,
            LeadEmployeeId = dto.LeadEmployeeId,
            CustomerPONumber = TrimOrNull(dto.CustomerPONumber),
            InvoiceDate = dto.InvoiceDate,
            AccountingDate = dto.AccountingDate,
            DueDate = dto.DueDate,
            NetTermId = dto.NetTermId,
            PreferredPaymentMethodId = dto.PreferredPaymentMethodId,
            LaborPricingMatrixId = dto.LaborPricingMatrixId,
            MaterialPricingMatrixId = dto.MaterialPricingMatrixId,
            OtherPricingMatrixId = dto.OtherPricingMatrixId,
            GLBreak1Id = dto.GLBreak1Id,
            GLBreak2Id = dto.GLBreak2Id,
            TaxingAuthorityJson = TrimOrNull(dto.TaxingAuthorityJson),
            BillToAddressJson = TrimOrNull(dto.BillToAddressJson),
            ServiceLocationAddressJson = TrimOrNull(dto.ServiceLocationAddressJson),
            CompanyAddressJson = TrimOrNull(dto.CompanyAddressJson),
            InvoiceTemplateId = dto.InvoiceTemplateId,
            InvoiceSubtotal = dto.InvoiceSubtotal,
            TotalDiscount = dto.TotalDiscount,
            TaxableAmount = dto.TaxableAmount,
            TotalTax = dto.TotalTax,
            InvoiceTotal = dto.InvoiceTotal,
            BalanceDue = dto.BalanceDue
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsInvoices.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return FgsInvoiceReadRepository.MapToDetail(entity);
    }

    public async Task<FgsInvoiceDetailDto> UpdateAsync(
        long id,
        FgsInvoiceUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Invoice '{id}' was not found.");

        ApplyMutableFields(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return FgsInvoiceReadRepository.MapToDetail(entity);
    }

    public async Task<FgsInvoiceDetailDto> PatchAsync(
        long id,
        FgsInvoicePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Invoice '{id}' was not found.");

        if (dto.InvoiceNumber is not null)
        {
            entity.InvoiceNumber = NormalizeInvoiceNumber(dto.InvoiceNumber);
        }

        if (dto.InvoiceTypeId.HasValue)
        {
            entity.InvoiceTypeId = dto.InvoiceTypeId.Value;
        }

        if (dto.CustomerId.HasValue)
        {
            entity.CustomerId = dto.CustomerId.Value;
        }

        if (dto.ServiceLocationId.HasValue)
        {
            entity.ServiceLocationId = dto.ServiceLocationId.Value;
        }

        if (dto.WorkOrderId.HasValue)
        {
            entity.WorkOrderId = dto.WorkOrderId;
        }

        if (dto.ProjectId.HasValue)
        {
            entity.ProjectId = dto.ProjectId;
        }

        if (dto.ServiceAgreementId.HasValue)
        {
            entity.ServiceAgreementId = dto.ServiceAgreementId;
        }

        if (dto.MaintenanceVisitId.HasValue)
        {
            entity.MaintenanceVisitId = dto.MaintenanceVisitId;
        }

        if (dto.ServiceJobNum is not null)
        {
            entity.ServiceJobNum = TrimOrNull(dto.ServiceJobNum);
        }

        if (dto.IsAgreementBilling.HasValue)
        {
            entity.IsAgreementBilling = dto.IsAgreementBilling.Value;
        }

        if (dto.IsRecurringInvoice.HasValue)
        {
            entity.IsRecurringInvoice = dto.IsRecurringInvoice.Value;
        }

        if (dto.RecurringScheduleId.HasValue)
        {
            entity.RecurringScheduleId = dto.RecurringScheduleId;
        }

        if (dto.WorkOrderNumber is not null)
        {
            entity.WorkOrderNumber = TrimOrNull(dto.WorkOrderNumber);
        }

        if (dto.JobTypeId.HasValue)
        {
            entity.JobTypeId = dto.JobTypeId;
        }

        if (dto.LeadEmployeeId.HasValue)
        {
            entity.LeadEmployeeId = dto.LeadEmployeeId;
        }

        if (dto.CustomerPONumber is not null)
        {
            entity.CustomerPONumber = TrimOrNull(dto.CustomerPONumber);
        }

        if (dto.InvoiceDate.HasValue)
        {
            entity.InvoiceDate = dto.InvoiceDate.Value;
        }

        if (dto.AccountingDate.HasValue)
        {
            entity.AccountingDate = dto.AccountingDate.Value;
        }

        if (dto.DueDate.HasValue)
        {
            entity.DueDate = dto.DueDate;
        }

        if (dto.NetTermId.HasValue)
        {
            entity.NetTermId = dto.NetTermId;
        }

        if (dto.PreferredPaymentMethodId.HasValue)
        {
            entity.PreferredPaymentMethodId = dto.PreferredPaymentMethodId;
        }

        if (dto.LaborPricingMatrixId.HasValue)
        {
            entity.LaborPricingMatrixId = dto.LaborPricingMatrixId;
        }

        if (dto.MaterialPricingMatrixId.HasValue)
        {
            entity.MaterialPricingMatrixId = dto.MaterialPricingMatrixId;
        }

        if (dto.OtherPricingMatrixId.HasValue)
        {
            entity.OtherPricingMatrixId = dto.OtherPricingMatrixId;
        }

        if (dto.GLBreak1Id.HasValue)
        {
            entity.GLBreak1Id = dto.GLBreak1Id;
        }

        if (dto.GLBreak2Id.HasValue)
        {
            entity.GLBreak2Id = dto.GLBreak2Id;
        }

        if (dto.TaxingAuthorityJson is not null)
        {
            entity.TaxingAuthorityJson = TrimOrNull(dto.TaxingAuthorityJson);
        }

        if (dto.BillToAddressJson is not null)
        {
            entity.BillToAddressJson = TrimOrNull(dto.BillToAddressJson);
        }

        if (dto.ServiceLocationAddressJson is not null)
        {
            entity.ServiceLocationAddressJson = TrimOrNull(dto.ServiceLocationAddressJson);
        }

        if (dto.CompanyAddressJson is not null)
        {
            entity.CompanyAddressJson = TrimOrNull(dto.CompanyAddressJson);
        }

        if (dto.InvoiceTemplateId.HasValue)
        {
            entity.InvoiceTemplateId = dto.InvoiceTemplateId;
        }

        if (dto.InvoiceSubtotal.HasValue)
        {
            entity.InvoiceSubtotal = dto.InvoiceSubtotal.Value;
        }

        if (dto.TotalDiscount.HasValue)
        {
            entity.TotalDiscount = dto.TotalDiscount.Value;
        }

        if (dto.TaxableAmount.HasValue)
        {
            entity.TaxableAmount = dto.TaxableAmount.Value;
        }

        if (dto.TotalTax.HasValue)
        {
            entity.TotalTax = dto.TotalTax.Value;
        }

        if (dto.InvoiceTotal.HasValue)
        {
            entity.InvoiceTotal = dto.InvoiceTotal.Value;
        }

        if (dto.BalanceDue.HasValue)
        {
            entity.BalanceDue = dto.BalanceDue.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return FgsInvoiceReadRepository.MapToDetail(entity);
    }

    private async Task<FgsInvoice?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsInvoices.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("An invoice with the same number already exists.", ex);
        }
    }

    private static void ApplyMutableFields(FgsInvoice entity, FgsInvoiceUpdateDto dto)
    {
        entity.InvoiceNumber = NormalizeInvoiceNumber(dto.InvoiceNumber);
        entity.InvoiceTypeId = dto.InvoiceTypeId;
        entity.CustomerId = dto.CustomerId;
        entity.ServiceLocationId = dto.ServiceLocationId;
        entity.WorkOrderId = dto.WorkOrderId;
        entity.ProjectId = dto.ProjectId;
        entity.ServiceAgreementId = dto.ServiceAgreementId;
        entity.MaintenanceVisitId = dto.MaintenanceVisitId;
        entity.ServiceJobNum = TrimOrNull(dto.ServiceJobNum);
        entity.IsAgreementBilling = dto.IsAgreementBilling;
        entity.IsRecurringInvoice = dto.IsRecurringInvoice;
        entity.RecurringScheduleId = dto.RecurringScheduleId;
        entity.WorkOrderNumber = TrimOrNull(dto.WorkOrderNumber);
        entity.JobTypeId = dto.JobTypeId;
        entity.LeadEmployeeId = dto.LeadEmployeeId;
        entity.CustomerPONumber = TrimOrNull(dto.CustomerPONumber);
        entity.InvoiceDate = dto.InvoiceDate;
        entity.AccountingDate = dto.AccountingDate;
        entity.DueDate = dto.DueDate;
        entity.NetTermId = dto.NetTermId;
        entity.PreferredPaymentMethodId = dto.PreferredPaymentMethodId;
        entity.LaborPricingMatrixId = dto.LaborPricingMatrixId;
        entity.MaterialPricingMatrixId = dto.MaterialPricingMatrixId;
        entity.OtherPricingMatrixId = dto.OtherPricingMatrixId;
        entity.GLBreak1Id = dto.GLBreak1Id;
        entity.GLBreak2Id = dto.GLBreak2Id;
        entity.TaxingAuthorityJson = TrimOrNull(dto.TaxingAuthorityJson);
        entity.BillToAddressJson = TrimOrNull(dto.BillToAddressJson);
        entity.ServiceLocationAddressJson = TrimOrNull(dto.ServiceLocationAddressJson);
        entity.CompanyAddressJson = TrimOrNull(dto.CompanyAddressJson);
        entity.InvoiceTemplateId = dto.InvoiceTemplateId;
        entity.InvoiceSubtotal = dto.InvoiceSubtotal;
        entity.TotalDiscount = dto.TotalDiscount;
        entity.TaxableAmount = dto.TaxableAmount;
        entity.TotalTax = dto.TotalTax;
        entity.InvoiceTotal = dto.InvoiceTotal;
        entity.BalanceDue = dto.BalanceDue;
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeInvoiceNumber(string invoiceNumber) =>
        invoiceNumber.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
