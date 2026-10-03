using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Fgs.Crm.Infrastructure.Common;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Crm.Infrastructure.Persistence.Customers;

public sealed class CrmCustomerWriteService : ICrmCustomerWriteService
{
    private readonly FgsCrmDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CrmEntityAuditHelper _auditHelper;

    public CrmCustomerWriteService(
        FgsCrmDbContext context,
        IUnitOfWork unitOfWork,
        CrmEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<CrmCustomerCreateResultDto> CreateAsync(
        CrmCustomerCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        EnsureCreationOption(dto);
        EnsurePrimaryContact(dto);
        EnsureDistinctTagIds(dto.TagIds);
        if (dto.ServiceLocation is not null)
        {
            EnsureLocationContact(dto.ServiceLocation);
            EnsureDistinctTagIds(dto.ServiceLocation.TagIds);
        }

        var entity = new CrmCustomer
        {
            CustomerNumber = NormalizeCustomerNumber(dto.CustomerNumber),
            Name = dto.Name.Trim(),
            DisplayName = dto.DisplayName.Trim(),
            AddressLine1 = TrimOrNull(dto.AddressLine1),
            AddressLine2 = TrimOrNull(dto.AddressLine2),
            AddressLine3 = TrimOrNull(dto.AddressLine3),
            AddressLine4 = TrimOrNull(dto.AddressLine4),
            City = TrimOrNull(dto.City),
            State = TrimOrNull(dto.State),
            County = TrimOrNull(dto.County),
            Country = TrimOrNull(dto.Country),
            PostalCode = TrimOrNull(dto.PostalCode),
            FormattedAddress = TrimOrNull(dto.FormattedAddress),
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            PlaceId = TrimOrNull(dto.PlaceId),
            DefaultPaymentTermId = dto.DefaultPaymentTermId,
            DefaultPORequired = dto.DefaultPORequired,
            TaxExempt = dto.TaxExempt,
            TaxExemptNumber = TrimOrNull(dto.TaxExemptNumber),
            CustomerAccountNumber = TrimOrNull(dto.CustomerAccountNumber),
            ExternalEntityId = TrimOrNull(dto.ExternalEntityId),
            ExternalVersion = TrimOrNull(dto.ExternalVersion),
            Website = TrimOrNull(dto.Website)
        };

        _auditHelper.StampForCreate(entity);
        await ApplyCustomerPricingDefaultsAsync(entity, dto, cancellationToken);

        await _context.CrmCustomers.AddAsync(entity, cancellationToken);

        AddPrimaryContact(entity, dto);
        CrmServiceLocation? location = null;
        if (dto.CreationOption == CrmCustomerCreationOption.BillToAndServiceLocation)
        {
            location = await AddServiceLocationAsync(entity, dto.ServiceLocation!, cancellationToken);
        }

        if (HasTagIds(dto.TagIds) || (location is not null && HasTagIds(dto.ServiceLocation?.TagIds)))
        {
            await SaveWithTagsAsync(() =>
            {
                AddEntityTags(entity.Id, CrmTaggedEntityType.Customer, dto.TagIds);
                if (location is not null)
                {
                    AddEntityTags(location.Id, CrmTaggedEntityType.ServiceLocation, dto.ServiceLocation!.TagIds);
                }
            }, cancellationToken);
        }
        else
        {
            await SaveChangesAsync(cancellationToken);
        }

        return new CrmCustomerCreateResultDto(
            await MapToDetailAsync(entity, cancellationToken),
            location?.Id);
    }

    public async Task<CrmServiceLocationCreatedDto> AddServiceLocationAsync(
        long customerId,
        CrmServiceLocationCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        EnsureLocationContact(dto);
        EnsureDistinctTagIds(dto.TagIds);

        var entity = await FindEntityAsync(customerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer '{customerId}' was not found.");

        var location = await AddServiceLocationAsync(entity, dto, cancellationToken);
        if (HasTagIds(dto.TagIds))
        {
            await SaveWithTagsAsync(
                () => AddEntityTags(location.Id, CrmTaggedEntityType.ServiceLocation, dto.TagIds),
                cancellationToken);
        }
        else
        {
            await SaveChangesAsync(cancellationToken);
        }

        return new CrmServiceLocationCreatedDto(
            location.Id,
            entity.Id,
            location.LocationSequence,
            location.LocationNumber);
    }

    public async Task<CrmCustomerDetailDto> UpdateAsync(
        long id,
        CrmCustomerUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer '{id}' was not found.");

        ApplyMutableFields(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity, cancellationToken);
    }

    public async Task<CrmCustomerDetailDto> PatchAsync(
        long id,
        CrmCustomerPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer '{id}' was not found.");

        if (dto.CustomerNumber is not null)
        {
            entity.CustomerNumber = NormalizeCustomerNumber(dto.CustomerNumber);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.DisplayName is not null)
        {
            entity.DisplayName = dto.DisplayName.Trim();
        }

        if (dto.AddressLine1 is not null)
        {
            entity.AddressLine1 = TrimOrNull(dto.AddressLine1);
        }

        if (dto.AddressLine2 is not null)
        {
            entity.AddressLine2 = TrimOrNull(dto.AddressLine2);
        }

        if (dto.AddressLine3 is not null)
        {
            entity.AddressLine3 = TrimOrNull(dto.AddressLine3);
        }

        if (dto.AddressLine4 is not null)
        {
            entity.AddressLine4 = TrimOrNull(dto.AddressLine4);
        }

        if (dto.City is not null)
        {
            entity.City = TrimOrNull(dto.City);
        }

        if (dto.State is not null)
        {
            entity.State = TrimOrNull(dto.State);
        }

        if (dto.County is not null)
        {
            entity.County = TrimOrNull(dto.County);
        }

        if (dto.Country is not null)
        {
            entity.Country = TrimOrNull(dto.Country);
        }

        if (dto.PostalCode is not null)
        {
            entity.PostalCode = TrimOrNull(dto.PostalCode);
        }

        if (dto.FormattedAddress is not null)
        {
            entity.FormattedAddress = TrimOrNull(dto.FormattedAddress);
        }

        if (dto.Latitude.HasValue)
        {
            entity.Latitude = dto.Latitude;
        }

        if (dto.Longitude.HasValue)
        {
            entity.Longitude = dto.Longitude;
        }

        if (dto.PlaceId is not null)
        {
            entity.PlaceId = TrimOrNull(dto.PlaceId);
        }

        if (dto.DefaultPaymentTermId.HasValue)
        {
            entity.DefaultPaymentTermId = dto.DefaultPaymentTermId;
        }

        if (dto.DefaultMaterialPricingMatrixId.HasValue)
        {
            entity.DefaultMaterialPricingMatrixId = dto.DefaultMaterialPricingMatrixId;
        }

        if (dto.DefaultLaborPricingMatrixId.HasValue)
        {
            entity.DefaultLaborPricingMatrixId = dto.DefaultLaborPricingMatrixId;
        }

        if (dto.DefaultOtherPricingMatrixId.HasValue)
        {
            entity.DefaultOtherPricingMatrixId = dto.DefaultOtherPricingMatrixId;
        }

        if (dto.DefaultPORequired.HasValue)
        {
            entity.DefaultPORequired = dto.DefaultPORequired.Value;
        }

        if (dto.TaxExempt.HasValue)
        {
            entity.TaxExempt = dto.TaxExempt.Value;
        }

        if (dto.TaxExemptNumber is not null)
        {
            entity.TaxExemptNumber = TrimOrNull(dto.TaxExemptNumber);
        }

        if (dto.CustomerAccountNumber is not null)
        {
            entity.CustomerAccountNumber = TrimOrNull(dto.CustomerAccountNumber);
        }

        if (dto.ExternalEntityId is not null)
        {
            entity.ExternalEntityId = TrimOrNull(dto.ExternalEntityId);
        }

        if (dto.ExternalVersion is not null)
        {
            entity.ExternalVersion = TrimOrNull(dto.ExternalVersion);
        }

        if (dto.Website is not null)
        {
            entity.Website = TrimOrNull(dto.Website);
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity, cancellationToken);
    }

    private async Task<CrmCustomer?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.CrmCustomers.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A customer with the same number already exists.", ex);
        }
    }

    private static void ApplyMutableFields(CrmCustomer entity, CrmCustomerUpdateDto dto)
    {
        entity.CustomerNumber = NormalizeCustomerNumber(dto.CustomerNumber);
        entity.Name = dto.Name.Trim();
        entity.DisplayName = dto.DisplayName.Trim();
        entity.AddressLine1 = TrimOrNull(dto.AddressLine1);
        entity.AddressLine2 = TrimOrNull(dto.AddressLine2);
        entity.AddressLine3 = TrimOrNull(dto.AddressLine3);
        entity.AddressLine4 = TrimOrNull(dto.AddressLine4);
        entity.City = TrimOrNull(dto.City);
        entity.State = TrimOrNull(dto.State);
        entity.County = TrimOrNull(dto.County);
        entity.Country = TrimOrNull(dto.Country);
        entity.PostalCode = TrimOrNull(dto.PostalCode);
        entity.FormattedAddress = TrimOrNull(dto.FormattedAddress);
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;
        entity.PlaceId = TrimOrNull(dto.PlaceId);
        entity.DefaultPaymentTermId = dto.DefaultPaymentTermId;
        entity.DefaultMaterialPricingMatrixId = dto.DefaultMaterialPricingMatrixId;
        entity.DefaultLaborPricingMatrixId = dto.DefaultLaborPricingMatrixId;
        entity.DefaultOtherPricingMatrixId = dto.DefaultOtherPricingMatrixId;
        entity.DefaultPORequired = dto.DefaultPORequired;
        entity.TaxExempt = dto.TaxExempt;
        entity.TaxExemptNumber = TrimOrNull(dto.TaxExemptNumber);
        entity.CustomerAccountNumber = TrimOrNull(dto.CustomerAccountNumber);
        entity.ExternalEntityId = TrimOrNull(dto.ExternalEntityId);
        entity.ExternalVersion = TrimOrNull(dto.ExternalVersion);
        entity.Website = TrimOrNull(dto.Website);
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCustomerNumber(string customerNumber) =>
        customerNumber.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private async Task ApplyCustomerPricingDefaultsAsync(
        CrmCustomer entity,
        CrmCustomerCreateDto dto,
        CancellationToken cancellationToken)
    {
        var defaults = await _context.CrmDefaultCustomers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                d => d.TenantId == entity.TenantId && d.CompanyId == entity.CompanyId,
                cancellationToken);

        entity.DefaultMaterialPricingMatrixId = dto.DefaultMaterialPricingMatrixId ?? defaults?.DefaultMaterialPricingMatrixId;
        entity.DefaultLaborPricingMatrixId = dto.DefaultLaborPricingMatrixId ?? defaults?.DefaultLaborPricingMatrixId;
        entity.DefaultOtherPricingMatrixId = dto.DefaultOtherPricingMatrixId ?? defaults?.DefaultOtherPricingMatrixId;
    }

    private void AddPrimaryContact(CrmCustomer customer, CrmCustomerCreateDto dto)
    {
        if (!HasPrimaryContactInput(dto))
        {
            return;
        }

        var contact = new CrmContact
        {
            CustomerId = customer.Id,
            ServiceLocationId = null,
            DisplayName = dto.PrimaryContactName!.Trim(),
            IsDefaultContact = true,
            IsActive = true
        };
        _auditHelper.StampForCreate(contact);
        _context.CrmContacts.Add(contact);

        AddCommunication(contact, ContactCommunicationType.Email, dto.PrimaryContactEmail);
        AddCommunication(contact, ContactCommunicationType.Phone, dto.PrimaryContactPhone);
    }

    private void AddCommunication(CrmContact contact, ContactCommunicationType type, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var communication = new CrmContactCommunication
        {
            ContactId = contact.Id,
            CommunicationTypeId = (short)type,
            Value = value.Trim(),
            IsPrimary = true,
            IsActive = true
        };
        _auditHelper.StampForCreate(communication);
        _context.CrmContactCommunications.Add(communication);
    }

    private async Task<CrmServiceLocation> AddServiceLocationAsync(
        CrmCustomer customer,
        CrmServiceLocationCreateDto dto,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(dto.ServiceLocationType))
        {
            throw new ArgumentException("ServiceLocationType must be between 1 and 5.");
        }

        var defaults = await _context.CrmDefaultServiceLocations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                d => d.TenantId == customer.TenantId && d.CompanyId == customer.CompanyId,
                cancellationToken);

        var sequence = customer.LastServiceLocationSequence + 1;
        customer.LastServiceLocationSequence = sequence;

        var location = new CrmServiceLocation
        {
            CustomerId = customer.Id,
            LocationSequence = sequence,
            LocationNumber = BuildLocationNumber(customer.CustomerNumber, sequence),
            Name = EmptyIfBlank(dto.Name),
            DisplayName = EmptyIfBlank(dto.DisplayName),
            ServiceLocationType = dto.ServiceLocationType,
            CustomerType = dto.CustomerType,
            AddressLine1 = TrimOrNull(dto.AddressLine1),
            AddressLine2 = TrimOrNull(dto.AddressLine2),
            AddressLine3 = TrimOrNull(dto.AddressLine3),
            AddressLine4 = TrimOrNull(dto.AddressLine4),
            City = TrimOrNull(dto.City),
            State = TrimOrNull(dto.State),
            County = TrimOrNull(dto.County),
            Country = TrimOrNull(dto.Country),
            PostalCode = TrimOrNull(dto.PostalCode),
            FormattedAddress = TrimOrNull(dto.FormattedAddress),
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            PlaceId = TrimOrNull(dto.PlaceId),
            DefaultPaymentMethodId = defaults?.DefaultPaymentMethodId,
            DefaultMaterialPricingMatrixId = defaults?.DefaultMaterialPricingMatrixId,
            DefaultLaborPricingMatrixId = defaults?.DefaultLaborPricingMatrixId,
            DefaultOtherPricingMatrixId = defaults?.DefaultOtherPricingMatrixId,
            InvoiceEmailTemplateId = defaults?.InvoiceEmailTemplateId,
            EstimateEmailTemplateId = defaults?.EstimateEmailTemplateId,
            InvoiceSmsTemplateId = defaults?.InvoiceSmsTemplateId,
            EstimateSmsTemplateId = defaults?.EstimateSmsTemplateId,
            EmailAllowed = defaults?.EmailAllowed ?? true,
            SmsAllowed = defaults?.SmsAllowed ?? true,
            TaxExempt = dto.TaxExempt ?? defaults?.TaxExempt ?? false,
            IsActive = true
        };

        _auditHelper.StampForCreate(location);
        await _context.CrmServiceLocations.AddAsync(location, cancellationToken);
        AddLocationPrimaryContact(location, dto);
        return location;
    }

    private void AddLocationPrimaryContact(CrmServiceLocation location, CrmServiceLocationCreateDto dto)
    {
        if (!HasLocationContactInput(dto))
        {
            return;
        }

        var contact = new CrmContact
        {
            CustomerId = null,
            ServiceLocationId = location.Id,
            DisplayName = dto.PrimaryContactName!.Trim(),
            IsDefaultContact = true,
            IsActive = true,
            CanReceiveEstimates = dto.CanReceiveEstimates ?? false,
            CanReceiveInvoices = dto.CanReceiveInvoices ?? false,
            CanReceiveAppointments = dto.CanReceiveAppointments ?? true
        };
        _auditHelper.StampForCreate(contact);
        _context.CrmContacts.Add(contact);

        AddCommunication(contact, ContactCommunicationType.Email, dto.PrimaryContactEmail);
        AddCommunication(contact, ContactCommunicationType.Phone, dto.PrimaryContactPhone);
    }

    private void AddEntityTags(long entityId, CrmTaggedEntityType entityType, IReadOnlyList<long>? tagIds)
    {
        if (!HasTagIds(tagIds))
        {
            return;
        }

        for (var index = 0; index < tagIds!.Count; index++)
        {
            var tag = new CrmEntityTag
            {
                TagId = tagIds[index],
                EntityTypeId = (int)entityType,
                EntityId = entityId,
                DisplayOrder = (short)index
            };
            _auditHelper.StampForCreate(tag);
            _context.CrmEntityTags.Add(tag);
        }
    }

    private async Task SaveWithTagsAsync(Action addTagsAfterIds, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
                await _context.SaveChangesAsync(ct);
                addTagsAfterIds();
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A customer with the same number already exists.", ex);
        }
    }

    private async Task<CrmCustomerDetailDto> MapToDetailAsync(
        CrmCustomer entity,
        CancellationToken cancellationToken)
    {
        var contact = await _context.CrmContacts
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.CustomerId == entity.Id && c.IsDefaultContact && c.ServiceLocationId == null,
                cancellationToken);

        var tagIds = await LoadCustomerTagIdsAsync(entity.Id, cancellationToken);
        if (contact is null)
        {
            return MapToDetail(entity, null, tagIds: tagIds);
        }

        var communications = await _context.CrmContactCommunications
            .AsNoTracking()
            .Where(cc => cc.ContactId == contact.Id && cc.IsPrimary)
            .ToListAsync(cancellationToken);

        return MapToDetail(entity, contact, communications, tagIds);
    }

    private async Task<IReadOnlyList<long>> LoadCustomerTagIdsAsync(
        long customerId,
        CancellationToken cancellationToken) =>
        await _context.CrmEntityTags
            .AsNoTracking()
            .Where(t => t.EntityTypeId == (int)CrmTaggedEntityType.Customer && t.EntityId == customerId)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.TagId)
            .Select(t => t.TagId)
            .ToListAsync(cancellationToken);

    private static void EnsureCreationOption(CrmCustomerCreateDto dto)
    {
        if (!Enum.IsDefined(dto.CreationOption))
        {
            throw new ArgumentException("CreationOption is invalid.");
        }

        if (dto.CreationOption == CrmCustomerCreationOption.BillToOnly && dto.ServiceLocation is not null)
        {
            throw new ArgumentException("ServiceLocation must be omitted when CreationOption is BillToOnly.");
        }

        if (dto.CreationOption == CrmCustomerCreationOption.BillToAndServiceLocation && dto.ServiceLocation is null)
        {
            throw new ArgumentException("ServiceLocation is required when CreationOption is BillToAndServiceLocation.");
        }
    }

    private static void EnsurePrimaryContact(CrmCustomerCreateDto dto)
    {
        var hasEmail = !string.IsNullOrWhiteSpace(dto.PrimaryContactEmail);
        var hasPhone = !string.IsNullOrWhiteSpace(dto.PrimaryContactPhone);
        if ((hasEmail || hasPhone) && string.IsNullOrWhiteSpace(dto.PrimaryContactName))
        {
            throw new ArgumentException("Primary contact name is required when email or phone is provided.");
        }
    }

    private static bool HasPrimaryContactInput(CrmCustomerCreateDto dto) =>
        !string.IsNullOrWhiteSpace(dto.PrimaryContactName)
        || !string.IsNullOrWhiteSpace(dto.PrimaryContactEmail)
        || !string.IsNullOrWhiteSpace(dto.PrimaryContactPhone);

    private static void EnsureLocationContact(CrmServiceLocationCreateDto dto)
    {
        var hasEmail = !string.IsNullOrWhiteSpace(dto.PrimaryContactEmail);
        var hasPhone = !string.IsNullOrWhiteSpace(dto.PrimaryContactPhone);
        if ((hasEmail || hasPhone) && string.IsNullOrWhiteSpace(dto.PrimaryContactName))
        {
            throw new ArgumentException("Primary contact name is required when email or phone is provided.");
        }
    }

    private static bool HasLocationContactInput(CrmServiceLocationCreateDto dto) =>
        !string.IsNullOrWhiteSpace(dto.PrimaryContactName)
        || !string.IsNullOrWhiteSpace(dto.PrimaryContactEmail)
        || !string.IsNullOrWhiteSpace(dto.PrimaryContactPhone);

    private static void EnsureDistinctTagIds(IReadOnlyList<long>? tagIds)
    {
        if (tagIds is not { Count: > 0 })
        {
            return;
        }

        if (tagIds.Distinct().Count() != tagIds.Count)
        {
            throw new ArgumentException("TagIds must not contain duplicates.");
        }
    }

    private static bool HasTagIds(IReadOnlyList<long>? tagIds) =>
        tagIds is { Count: > 0 };

    private static string BuildLocationNumber(string customerNumber, int sequence)
    {
        var value = $"{customerNumber}-{sequence}";
        return value.Length <= 50 ? value : value[..50];
    }

    private static string EmptyIfBlank(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    private static CrmCustomerDetailDto MapToDetail(
        CrmCustomer entity,
        CrmContact? contact,
        IReadOnlyList<CrmContactCommunication>? communications = null,
        IReadOnlyList<long>? tagIds = null)
    {
        var email = communications?
            .FirstOrDefault(cc => cc.CommunicationTypeId == (short)ContactCommunicationType.Email)?.Value;
        var phone = communications?
            .FirstOrDefault(cc => cc.CommunicationTypeId == (short)ContactCommunicationType.Phone)?.Value;

        return new CrmCustomerDetailDto(
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
            contact?.DisplayName,
            email,
            phone,
            entity.CreatedOn,
            tagIds ?? []);
    }
}
