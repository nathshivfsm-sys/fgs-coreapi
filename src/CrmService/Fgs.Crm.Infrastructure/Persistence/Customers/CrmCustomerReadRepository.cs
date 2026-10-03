using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
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

        if (entity is null)
        {
            return null;
        }

        var contact = await LoadDefaultContactAsync(entity.Id, cancellationToken);
        var tagIds = await LoadCustomerTagIdsAsync(entity.Id, cancellationToken);
        return MapToDetail(entity, contact.Name, contact.Email, contact.Phone, tagIds);
    }

    public async Task<CrmServiceLocationListResultDto?> ListServiceLocationsAsync(
        long customerId,
        CrmListQuery query,
        bool includeSummary = true,
        CancellationToken cancellationToken = default)
    {
        var customerExists = await dbContext.CrmCustomers
            .AsNoTracking()
            .AnyAsync(e => e.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            return null;
        }

        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.CrmServiceLocations
            .AsNoTracking()
            .Where(e => e.CustomerId == customerId);

        if (paging.IsActive.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.IsActive == paging.IsActive.Value);
        }

        dbQuery = ApplyServiceLocationSearch(dbQuery, paging.Search);
        dbQuery = ApplyServiceLocationSort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var locations = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = await MapLocationSummariesAsync(locations, cancellationToken);
        var summary = includeSummary
            ? await BuildServiceLocationSummaryAsync(customerId, cancellationToken)
            : EmptyLocationSummary;

        return new CrmServiceLocationListResultDto(items, page, pageSize, totalCount, summary);
    }

    public async Task<CrmCustomerListResultDto> ListAsync(
        CrmListQuery query,
        CrmCustomerListFilters filters,
        bool includeSummary = true,
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

        if (filters.IsPreferred.HasValue)
        {
            var isPreferred = filters.IsPreferred.Value;
            dbQuery = dbQuery.Where(e => e.IsPreferredCustomer == isPreferred);
        }

        dbQuery = ApplySearch(dbQuery, paging.Search);
        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var customers = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = await MapSummariesAsync(customers, cancellationToken);
        var summary = includeSummary
            ? await BuildSummaryAsync(cancellationToken)
            : EmptySummary;

        return new CrmCustomerListResultDto(items, page, pageSize, totalCount, summary);
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

    private static readonly CrmCustomerListSummaryDto EmptySummary = new(0, 0, 0, 0, 0, null);

    private IQueryable<CrmCustomer> ApplySearch(IQueryable<CrmCustomer> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var term = search.Trim().ToLower();
        var emailType = (short)ContactCommunicationType.Email;
        var phoneType = (short)ContactCommunicationType.Phone;

        var contactNameIds = dbContext.CrmContacts
            .Where(c => c.CustomerId != null
                && c.ServiceLocationId == null
                && c.IsDefaultContact
                && c.DisplayName.ToLower().Contains(term))
            .Select(c => c.CustomerId!.Value);

        var communicationIds =
            from communication in dbContext.CrmContactCommunications
            join contact in dbContext.CrmContacts on communication.ContactId equals contact.Id
            where communication.IsPrimary
                && (communication.CommunicationTypeId == emailType || communication.CommunicationTypeId == phoneType)
                && communication.Value.ToLower().Contains(term)
                && contact.CustomerId != null
                && contact.ServiceLocationId == null
                && contact.IsDefaultContact
            select contact.CustomerId!.Value;

        return query.Where(e =>
            e.CustomerNumber.ToLower().Contains(term)
            || e.Name.ToLower().Contains(term)
            || (e.FormattedAddress != null && e.FormattedAddress.ToLower().Contains(term))
            || (e.AddressLine1 != null && e.AddressLine1.ToLower().Contains(term))
            || (e.AddressLine2 != null && e.AddressLine2.ToLower().Contains(term))
            || (e.AddressLine3 != null && e.AddressLine3.ToLower().Contains(term))
            || (e.AddressLine4 != null && e.AddressLine4.ToLower().Contains(term))
            || (e.PostalCode != null && e.PostalCode.ToLower().Contains(term))
            || contactNameIds.Contains(e.Id)
            || communicationIds.Contains(e.Id));
    }

    private IQueryable<CrmCustomer> ApplySort(
        IQueryable<CrmCustomer> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        var key = sortBy?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(key))
        {
            return desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
        }

        var emailType = (short)ContactCommunicationType.Email;
        var phoneType = (short)ContactCommunicationType.Phone;

        return key switch
        {
            "customernumber" => desc ? query.OrderByDescending(e => e.CustomerNumber) : query.OrderBy(e => e.CustomerNumber),
            "name" => desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
            "formattedaddress" => desc ? query.OrderByDescending(e => e.FormattedAddress) : query.OrderBy(e => e.FormattedAddress),
            "email" => OrderByCommunication(query, emailType, desc),
            "phone" => OrderByCommunication(query, phoneType, desc),
            "createdon" => desc ? query.OrderByDescending(e => e.CreatedOn) : query.OrderBy(e => e.CreatedOn),
            "displayname" => desc ? query.OrderByDescending(e => e.DisplayName) : query.OrderBy(e => e.DisplayName),
            "city" => desc ? query.OrderByDescending(e => e.City) : query.OrderBy(e => e.City),
            "state" => desc ? query.OrderByDescending(e => e.State) : query.OrderBy(e => e.State),
            "isactive" => desc ? query.OrderByDescending(e => e.IsActive) : query.OrderBy(e => e.IsActive),
            _ => desc ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id)
        };
    }

    private IQueryable<CrmCustomer> OrderByCommunication(
        IQueryable<CrmCustomer> query,
        short communicationTypeId,
        bool descending)
    {
        return descending
            ? query.OrderByDescending(e =>
                (from communication in dbContext.CrmContactCommunications
                 join contact in dbContext.CrmContacts on communication.ContactId equals contact.Id
                 where contact.CustomerId == e.Id
                     && contact.ServiceLocationId == null
                     && contact.IsDefaultContact
                     && communication.IsPrimary
                     && communication.CommunicationTypeId == communicationTypeId
                 select communication.Value).FirstOrDefault())
            : query.OrderBy(e =>
                (from communication in dbContext.CrmContactCommunications
                 join contact in dbContext.CrmContacts on communication.ContactId equals contact.Id
                 where contact.CustomerId == e.Id
                     && contact.ServiceLocationId == null
                     && contact.IsDefaultContact
                     && communication.IsPrimary
                     && communication.CommunicationTypeId == communicationTypeId
                 select communication.Value).FirstOrDefault());
    }

    private async Task<CrmCustomerListSummaryDto> BuildSummaryAsync(CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var monthStart = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var previousMonthStart = monthStart.AddMonths(-1);
        var customers = dbContext.CrmCustomers.AsNoTracking();

        var total = await customers.CountAsync(cancellationToken);
        var active = await customers.CountAsync(e => e.IsActive, cancellationToken);
        var inactive = await customers.CountAsync(e => !e.IsActive, cancellationToken);
        var newThisMonth = await customers.CountAsync(e => e.CreatedOn >= monthStart, cancellationToken);
        var previousMonth = await customers.CountAsync(
            e => e.CreatedOn >= previousMonthStart && e.CreatedOn < monthStart,
            cancellationToken);

        decimal? percent = previousMonth == 0
            ? null
            : (decimal)(newThisMonth - previousMonth) / previousMonth * 100m;

        return new CrmCustomerListSummaryDto(total, active, inactive, newThisMonth, previousMonth, percent);
    }

    private async Task<IReadOnlyList<CrmCustomerSummaryDto>> MapSummariesAsync(
        List<CrmCustomer> customers,
        CancellationToken cancellationToken)
    {
        if (customers.Count == 0)
        {
            return [];
        }

        var ids = customers.Select(c => c.Id).ToArray();
        var contacts = await dbContext.CrmContacts
            .AsNoTracking()
            .Where(c => c.CustomerId != null
                && ids.Contains(c.CustomerId.Value)
                && c.IsDefaultContact
                && c.ServiceLocationId == null)
            .ToListAsync(cancellationToken);

        var contactIds = contacts.Select(c => c.Id).ToArray();
        var communications = contactIds.Length == 0
            ? []
            : await dbContext.CrmContactCommunications
                .AsNoTracking()
                .Where(cc => contactIds.Contains(cc.ContactId)
                    && cc.IsPrimary
                    && (cc.CommunicationTypeId == (short)ContactCommunicationType.Email
                        || cc.CommunicationTypeId == (short)ContactCommunicationType.Phone))
                .ToListAsync(cancellationToken);

        var customerTypeId = (int)CrmTaggedEntityType.Customer;
        var tags = await dbContext.CrmEntityTags
            .AsNoTracking()
            .Where(t => t.EntityTypeId == customerTypeId && ids.Contains(t.EntityId))
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.TagId)
            .ToListAsync(cancellationToken);

        var contactByCustomer = contacts
            .GroupBy(c => c.CustomerId!.Value)
            .ToDictionary(g => g.Key, g => g.First());

        var tagsByEntity = tags
            .GroupBy(t => t.EntityId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<long>)g.Select(t => t.TagId).ToList());

        return customers.Select(customer =>
        {
            contactByCustomer.TryGetValue(customer.Id, out var contact);
            string? email = null;
            string? phone = null;
            if (contact is not null)
            {
                email = communications.FirstOrDefault(cc =>
                    cc.ContactId == contact.Id && cc.CommunicationTypeId == (short)ContactCommunicationType.Email)?.Value;
                phone = communications.FirstOrDefault(cc =>
                    cc.ContactId == contact.Id && cc.CommunicationTypeId == (short)ContactCommunicationType.Phone)?.Value;
            }

            tagsByEntity.TryGetValue(customer.Id, out var tagIds);
            return new CrmCustomerSummaryDto(
                customer.Id,
                customer.CustomerNumber,
                customer.Name,
                customer.DisplayName,
                customer.City,
                customer.State,
                customer.PostalCode,
                customer.Country,
                customer.CustomerAccountNumber,
                customer.IsActive,
                customer.FormattedAddress,
                contact?.DisplayName,
                email,
                phone,
                tagIds ?? [],
                customer.CreatedOn,
                customer.IsPreferredCustomer);
        }).ToList();
    }

    private async Task<(string? Name, string? Email, string? Phone)> LoadDefaultContactAsync(
        long customerId,
        CancellationToken cancellationToken)
    {
        var contact = await dbContext.CrmContacts
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.CustomerId == customerId && c.IsDefaultContact && c.ServiceLocationId == null,
                cancellationToken);

        if (contact is null)
        {
            return (null, null, null);
        }

        var communications = await dbContext.CrmContactCommunications
            .AsNoTracking()
            .Where(cc => cc.ContactId == contact.Id && cc.IsPrimary)
            .ToListAsync(cancellationToken);

        var email = communications
            .FirstOrDefault(cc => cc.CommunicationTypeId == (short)ContactCommunicationType.Email)?.Value;
        var phone = communications
            .FirstOrDefault(cc => cc.CommunicationTypeId == (short)ContactCommunicationType.Phone)?.Value;
        return (contact.DisplayName, email, phone);
    }

    private async Task<IReadOnlyList<long>> LoadCustomerTagIdsAsync(
        long customerId,
        CancellationToken cancellationToken) =>
        await dbContext.CrmEntityTags
            .AsNoTracking()
            .Where(t => t.EntityTypeId == (int)CrmTaggedEntityType.Customer && t.EntityId == customerId)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.TagId)
            .Select(t => t.TagId)
            .ToListAsync(cancellationToken);

    private static readonly CrmServiceLocationListSummaryDto EmptyLocationSummary = new(0, 0);

    private IQueryable<CrmServiceLocation> ApplyServiceLocationSearch(
        IQueryable<CrmServiceLocation> query,
        string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var term = search.Trim().ToLower();
        var emailType = (short)ContactCommunicationType.Email;
        var phoneType = (short)ContactCommunicationType.Phone;

        var communicationIds =
            from communication in dbContext.CrmContactCommunications
            join contact in dbContext.CrmContacts on communication.ContactId equals contact.Id
            where communication.IsPrimary
                && (communication.CommunicationTypeId == emailType || communication.CommunicationTypeId == phoneType)
                && communication.Value.ToLower().Contains(term)
                && contact.ServiceLocationId != null
                && contact.CustomerId == null
                && contact.IsDefaultContact
            select contact.ServiceLocationId!.Value;

        return query.Where(e =>
            e.LocationNumber.ToLower().Contains(term)
            || e.Name.ToLower().Contains(term)
            || e.DisplayName.ToLower().Contains(term)
            || (e.FormattedAddress != null && e.FormattedAddress.ToLower().Contains(term))
            || (e.AddressLine1 != null && e.AddressLine1.ToLower().Contains(term))
            || (e.AddressLine2 != null && e.AddressLine2.ToLower().Contains(term))
            || (e.AddressLine3 != null && e.AddressLine3.ToLower().Contains(term))
            || (e.AddressLine4 != null && e.AddressLine4.ToLower().Contains(term))
            || (e.PostalCode != null && e.PostalCode.ToLower().Contains(term))
            || communicationIds.Contains(e.Id));
    }

    private static IQueryable<CrmServiceLocation> ApplyServiceLocationSort(
        IQueryable<CrmServiceLocation> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        var key = sortBy?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(key))
        {
            return desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
        }

        return key switch
        {
            "locationnumber" => desc ? query.OrderByDescending(e => e.LocationNumber) : query.OrderBy(e => e.LocationNumber),
            "name" => desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
            "displayname" => desc ? query.OrderByDescending(e => e.DisplayName) : query.OrderBy(e => e.DisplayName),
            "formattedaddress" => desc
                ? query.OrderByDescending(e => e.FormattedAddress)
                : query.OrderBy(e => e.FormattedAddress),
            _ => desc ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id)
        };
    }

    private async Task<CrmServiceLocationListSummaryDto> BuildServiceLocationSummaryAsync(
        long customerId,
        CancellationToken cancellationToken)
    {
        var locations = dbContext.CrmServiceLocations
            .AsNoTracking()
            .Where(e => e.CustomerId == customerId);

        var activeCount = await locations.CountAsync(e => e.IsActive, cancellationToken);
        var inactiveCount = await locations.CountAsync(e => !e.IsActive, cancellationToken);
        return new CrmServiceLocationListSummaryDto(activeCount, inactiveCount);
    }

    private async Task<IReadOnlyList<CrmServiceLocationSummaryDto>> MapLocationSummariesAsync(
        List<CrmServiceLocation> locations,
        CancellationToken cancellationToken)
    {
        if (locations.Count == 0)
        {
            return [];
        }

        var ids = locations.Select(l => l.Id).ToArray();
        var contacts = await dbContext.CrmContacts
            .AsNoTracking()
            .Where(c => c.ServiceLocationId != null
                && ids.Contains(c.ServiceLocationId.Value)
                && c.CustomerId == null
                && c.IsDefaultContact)
            .ToListAsync(cancellationToken);

        var contactIds = contacts.Select(c => c.Id).ToArray();
        var communications = contactIds.Length == 0
            ? []
            : await dbContext.CrmContactCommunications
                .AsNoTracking()
                .Where(cc => contactIds.Contains(cc.ContactId)
                    && cc.IsPrimary
                    && (cc.CommunicationTypeId == (short)ContactCommunicationType.Email
                        || cc.CommunicationTypeId == (short)ContactCommunicationType.Phone))
                .ToListAsync(cancellationToken);

        var contactByLocation = contacts
            .GroupBy(c => c.ServiceLocationId!.Value)
            .ToDictionary(g => g.Key, g => g.First());

        return locations.Select(location =>
        {
            contactByLocation.TryGetValue(location.Id, out var contact);
            string? email = null;
            string? phone = null;
            if (contact is not null)
            {
                email = communications.FirstOrDefault(cc =>
                    cc.ContactId == contact.Id && cc.CommunicationTypeId == (short)ContactCommunicationType.Email)?.Value;
                phone = communications.FirstOrDefault(cc =>
                    cc.ContactId == contact.Id && cc.CommunicationTypeId == (short)ContactCommunicationType.Phone)?.Value;
            }

            return new CrmServiceLocationSummaryDto(
                location.Id,
                location.LocationNumber,
                location.Name,
                location.DisplayName,
                location.FormattedAddress,
                email,
                phone,
                location.IsActive,
                location.CustomerType);
        }).ToList();
    }

    private static CrmCustomerDetailDto MapToDetail(
        CrmCustomer entity,
        string? primaryContactName,
        string? primaryContactEmail,
        string? primaryContactPhone,
        IReadOnlyList<long> tagIds) =>
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
            entity.IsActive,
            entity.Website,
            primaryContactName,
            primaryContactEmail,
            primaryContactPhone,
            entity.CreatedOn,
            tagIds);
}
