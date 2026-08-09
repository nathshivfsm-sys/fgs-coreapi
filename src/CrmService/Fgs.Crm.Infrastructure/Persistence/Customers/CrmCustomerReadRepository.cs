using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Foundation.Paging;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Crm.Infrastructure.Persistence.Customers;

internal sealed class CrmCustomerReadRepository(FgsCrmDbContext dbContext) : ICrmCustomerReadRepository
{
    public async Task<CrmCustomerDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.CrmCustomers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToDetail(entity);
    }

    public async Task<PagedResult<CrmCustomerSummaryDto>> ListAsync(
        CrmListQuery query,
        CrmCustomerListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.CrmCustomers.AsNoTracking();

        if (paging.IsActive.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.IsActive == paging.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(filters.CustomerNumber))
        {
            var customerNumber = filters.CustomerNumber.Trim().ToUpperInvariant();
            dbQuery = dbQuery.Where(e => e.CustomerNumber == customerNumber);
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            var pattern = $"%{filters.Name.Trim()}%";
            dbQuery = dbQuery.Where(e => EF.Functions.ILike(e.Name, pattern));
        }

        if (!string.IsNullOrWhiteSpace(filters.DisplayName))
        {
            var pattern = $"%{filters.DisplayName.Trim()}%";
            dbQuery = dbQuery.Where(e => EF.Functions.ILike(e.DisplayName, pattern));
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            var pattern = $"%{paging.Search.Trim()}%";
            dbQuery = dbQuery.Where(e =>
                EF.Functions.ILike(e.CustomerNumber, pattern)
                || EF.Functions.ILike(e.Name, pattern)
                || EF.Functions.ILike(e.DisplayName, pattern)
                || (e.CustomerAccountNumber != null && EF.Functions.ILike(e.CustomerAccountNumber, pattern))
                || (e.City != null && EF.Functions.ILike(e.City, pattern)));
        }

        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var items = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new CrmCustomerSummaryDto(
                e.Id,
                e.CustomerNumber,
                e.Name,
                e.DisplayName,
                e.City,
                e.State,
                e.PostalCode,
                e.Country,
                e.CustomerAccountNumber,
                e.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResult<CrmCustomerSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<CrmCustomerLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var dbQuery = dbContext.CrmCustomers.AsNoTracking();

        if (activeOnly)
        {
            dbQuery = dbQuery.Where(e => e.IsActive);
        }

        return await dbQuery
            .OrderBy(e => e.DisplayName)
            .Select(e => new CrmCustomerLookupDto(e.Id, e.CustomerNumber, e.DisplayName))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCustomerNumberAsync(
        string customerNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = customerNumber.Trim().ToUpperInvariant();
        var dbQuery = dbContext.CrmCustomers.AsNoTracking()
            .Where(e => e.CustomerNumber == normalized);

        if (excludeId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.Id != excludeId.Value);
        }

        return await dbQuery.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var dbQuery = dbContext.CrmCustomers.AsNoTracking().Where(e => e.Id == id);
        if (activeOnly)
        {
            dbQuery = dbQuery.Where(e => e.IsActive);
        }

        return await dbQuery.AnyAsync(cancellationToken);
    }

    private static IQueryable<CrmCustomer> ApplySort(
        IQueryable<CrmCustomer> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        return (sortBy?.Trim().ToLowerInvariant()) switch
        {
            "customernumber" => desc ? query.OrderByDescending(e => e.CustomerNumber) : query.OrderBy(e => e.CustomerNumber),
            "name" => desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
            "displayname" => desc ? query.OrderByDescending(e => e.DisplayName) : query.OrderBy(e => e.DisplayName),
            "city" => desc ? query.OrderByDescending(e => e.City) : query.OrderBy(e => e.City),
            "state" => desc ? query.OrderByDescending(e => e.State) : query.OrderBy(e => e.State),
            "isactive" => desc ? query.OrderByDescending(e => e.IsActive) : query.OrderBy(e => e.IsActive),
            "createdon" => desc ? query.OrderByDescending(e => e.CreatedOn) : query.OrderBy(e => e.CreatedOn),
            _ => desc ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id)
        };
    }

    private static CrmCustomerDetailDto MapToDetail(CrmCustomer entity) =>
        new(
            entity.Id,
            entity.CustomerNumber,
            entity.Name,
            entity.DisplayName,
            entity.AddressLine1,
            entity.AddressLine2,
            entity.AddressLine3,
            entity.AddressLine4,
            entity.City,
            entity.State,
            entity.County,
            entity.Country,
            entity.PostalCode,
            entity.FormattedAddress,
            entity.Latitude,
            entity.Longitude,
            entity.PlaceId,
            entity.DefaultPaymentTermId,
            entity.DefaultMaterialPricingMatrixId,
            entity.DefaultLaborPricingMatrixId,
            entity.DefaultOtherPricingMatrixId,
            entity.DefaultPORequired,
            entity.TaxExempt,
            entity.TaxExemptNumber,
            entity.CustomerAccountNumber,
            entity.ExternalEntityId,
            entity.ExternalVersion,
            entity.IsActive);
}
