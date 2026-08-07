using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Foundation.Paging;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Billing.Infrastructure.Persistence.Invoices;

internal sealed class FgsInvoiceReadRepository(FgsBillingDbContext dbContext) : IFgsInvoiceReadRepository
{
    public async Task<FgsInvoiceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.FgsInvoices
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var lines = await dbContext.FgsInvoiceDetails
            .AsNoTracking()
            .Where(d => d.InvoiceId == id)
            .OrderBy(d => d.LineNumber)
            .Select(d => new FgsInvoiceLineDto(
                d.Id,
                d.LineNumber,
                d.BillingCategoryId,
                d.ItemCode,
                d.ItemDescription,
                d.Quantity,
                d.UnitPrice,
                d.ExtendedPrice,
                d.IsTaxable))
            .ToListAsync(cancellationToken);

        return MapToDetail(entity, lines);
    }

    public async Task<PagedResult<FgsInvoiceSummaryDto>> ListAsync(
        BillingListQuery query,
        FgsInvoiceListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.FgsInvoices.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filters.InvoiceNumber))
        {
            var invoiceNumber = filters.InvoiceNumber.Trim().ToUpperInvariant();
            dbQuery = dbQuery.Where(e => e.InvoiceNumber == invoiceNumber);
        }

        if (filters.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.CustomerId == filters.CustomerId.Value);
        }

        if (filters.IsPosted.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.IsPosted == filters.IsPosted.Value);
        }

        if (filters.IsApproved.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.IsApproved == filters.IsApproved.Value);
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            var pattern = $"%{paging.Search.Trim()}%";
            dbQuery = dbQuery.Where(e =>
                EF.Functions.ILike(e.InvoiceNumber, pattern)
                || (e.WorkOrderNumber != null && EF.Functions.ILike(e.WorkOrderNumber, pattern))
                || (e.CustomerPONumber != null && EF.Functions.ILike(e.CustomerPONumber, pattern))
                || (e.ServiceJobNum != null && EF.Functions.ILike(e.ServiceJobNum, pattern)));
        }

        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var items = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new FgsInvoiceSummaryDto(
                e.Id,
                e.InvoiceNumber,
                e.InvoiceTypeId,
                e.CustomerId,
                e.ServiceLocationId,
                e.InvoiceDate,
                e.DueDate,
                e.InvoiceTotal,
                e.BalanceDue,
                e.IsApproved,
                e.IsPosted))
            .ToListAsync(cancellationToken);

        return new PagedResult<FgsInvoiceSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsInvoiceLookupDto>> LookupAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.FgsInvoices
            .AsNoTracking()
            .OrderByDescending(e => e.InvoiceDate)
            .ThenBy(e => e.InvoiceNumber)
            .Select(e => new FgsInvoiceLookupDto(e.Id, e.InvoiceNumber))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByInvoiceNumberAsync(
        string invoiceNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = invoiceNumber.Trim().ToUpperInvariant();
        var dbQuery = dbContext.FgsInvoices.AsNoTracking()
            .Where(e => e.InvoiceNumber == normalized);

        if (excludeId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.Id != excludeId.Value);
        }

        return await dbQuery.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default) =>
        await dbContext.FgsInvoices.AsNoTracking().AnyAsync(e => e.Id == id, cancellationToken);

    private static IQueryable<FgsInvoice> ApplySort(
        IQueryable<FgsInvoice> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        return (sortBy?.Trim().ToLowerInvariant()) switch
        {
            "invoicenumber" => desc ? query.OrderByDescending(e => e.InvoiceNumber) : query.OrderBy(e => e.InvoiceNumber),
            "invoicedate" => desc ? query.OrderByDescending(e => e.InvoiceDate) : query.OrderBy(e => e.InvoiceDate),
            "duedate" => desc ? query.OrderByDescending(e => e.DueDate) : query.OrderBy(e => e.DueDate),
            "invoicetotal" => desc ? query.OrderByDescending(e => e.InvoiceTotal) : query.OrderBy(e => e.InvoiceTotal),
            "balancedue" => desc ? query.OrderByDescending(e => e.BalanceDue) : query.OrderBy(e => e.BalanceDue),
            "customerid" => desc ? query.OrderByDescending(e => e.CustomerId) : query.OrderBy(e => e.CustomerId),
            "isposted" => desc ? query.OrderByDescending(e => e.IsPosted) : query.OrderBy(e => e.IsPosted),
            "isapproved" => desc ? query.OrderByDescending(e => e.IsApproved) : query.OrderBy(e => e.IsApproved),
            "createdon" => desc ? query.OrderByDescending(e => e.CreatedOn) : query.OrderBy(e => e.CreatedOn),
            _ => desc ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id)
        };
    }

    internal static FgsInvoiceDetailDto MapToDetail(
        FgsInvoice entity,
        IReadOnlyList<FgsInvoiceLineDto>? lines = null) =>
        new(
            entity.Id,
            entity.InvoiceNumber,
            entity.InvoiceTypeId,
            entity.CustomerId,
            entity.ServiceLocationId,
            entity.WorkOrderId,
            entity.ProjectId,
            entity.ServiceAgreementId,
            entity.MaintenanceVisitId,
            entity.ServiceJobNum,
            entity.IsAgreementBilling,
            entity.IsRecurringInvoice,
            entity.RecurringScheduleId,
            entity.WorkOrderNumber,
            entity.JobTypeId,
            entity.LeadEmployeeId,
            entity.CustomerPONumber,
            entity.InvoiceDate,
            entity.AccountingDate,
            entity.DueDate,
            entity.NetTermId,
            entity.PreferredPaymentMethodId,
            entity.LaborPricingMatrixId,
            entity.MaterialPricingMatrixId,
            entity.OtherPricingMatrixId,
            entity.GLBreak1Id,
            entity.GLBreak2Id,
            entity.TaxingAuthorityJson,
            entity.BillToAddressJson,
            entity.ServiceLocationAddressJson,
            entity.CompanyAddressJson,
            entity.InvoiceTemplateId,
            entity.IsSigned,
            entity.SignedOn,
            entity.InvoiceSubtotal,
            entity.TotalDiscount,
            entity.TaxableAmount,
            entity.TotalTax,
            entity.InvoiceTotal,
            entity.AppliedAmount,
            entity.BalanceDue,
            entity.IsApproved,
            entity.ApprovedBy,
            entity.ApprovedOn,
            entity.IsPosted,
            entity.PostedBy,
            entity.PostedOn,
            entity.InvoiceBatchId,
            entity.ExternalAccountingId,
            entity.ExternalAccountingSyncToken,
            lines ?? Array.Empty<FgsInvoiceLineDto>());
}
