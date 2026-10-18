using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Fgs.Crm.Infrastructure.Common;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Crm.Infrastructure.Entities.Customers;

public sealed class CrmCustomerWriteService : ICrmCustomerWriteService
{
    private readonly FgsCrmDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CrmEntityAuditHelper _auditHelper;
    private readonly ICrmCustomerReadRepository _readRepository;

    public CrmCustomerWriteService(
        FgsCrmDbContext context,
        IUnitOfWork unitOfWork,
        CrmEntityAuditHelper auditHelper,
        ICrmCustomerReadRepository readRepository)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _readRepository = readRepository;
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
            TaxExemptNumber = TrimOrNull(dto.TaxExemptNumber),
            CustomerAccountNumber = TrimOrNull(dto.CustomerAccountNumber),
            ExternalEntityId = TrimOrNull(dto.ExternalEntityId),
            ExternalVersion = TrimOrNull(dto.ExternalVersion),
            Website = TrimOrNull(dto.Website)
        };

        _auditHelper.StampForCreate(entity);
        await ApplyCustomerPricingDefaultsAsync(entity, dto, cancellationToken);

        await _context.CrmCustomers.AddAsync(entity, cancellationToken);

        CrmServiceLocation? location = null;
        await SaveGraphAsync(async ct =>
        {
            await _context.SaveChangesAsync(ct);

            if (dto.CreationOption == CrmCustomerCreationOption.BillToAndServiceLocation)
            {
                location = await AddServiceLocationAsync(entity, dto.ServiceLocation!, ct);
                await _context.SaveChangesAsync(ct);
            }

            var customerContact = AddPrimaryContact(entity, dto);
            var locationContact = location is null
                ? null
                : AddLocationPrimaryContact(location, dto.ServiceLocation!);

            if (customerContact is not null || locationContact is not null)
            {
                await _context.SaveChangesAsync(ct);
            }

            if (customerContact is not null)
            {
                AddCommunication(customerContact, ContactCommunicationType.Email, dto.PrimaryContactEmail);
                AddCommunication(customerContact, ContactCommunicationType.Phone, dto.PrimaryContactPhone);
            }

            if (locationContact is not null)
            {
                AddCommunication(locationContact, ContactCommunicationType.Email, dto.ServiceLocation!.PrimaryContactEmail);
                AddCommunication(locationContact, ContactCommunicationType.Phone, dto.ServiceLocation.PrimaryContactPhone);
            }

            AddEntityTags(entity.Id, CrmTaggedEntityType.Customer, dto.TagIds);
            if (location is not null)
            {
                AddEntityTags(location.Id, CrmTaggedEntityType.ServiceLocation, dto.ServiceLocation!.TagIds);
            }
        }, cancellationToken);

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
        await SaveGraphAsync(async ct =>
        {
            await _context.SaveChangesAsync(ct);

            var contact = AddLocationPrimaryContact(location, dto);
            if (contact is not null)
            {
                await _context.SaveChangesAsync(ct);
                AddCommunication(contact, ContactCommunicationType.Email, dto.PrimaryContactEmail);
                AddCommunication(contact, ContactCommunicationType.Phone, dto.PrimaryContactPhone);
            }

            AddEntityTags(location.Id, CrmTaggedEntityType.ServiceLocation, dto.TagIds);
        }, cancellationToken);

        return new CrmServiceLocationCreatedDto(
            location.Id,
            entity.Id,
            location.LocationSequence,
            location.LocationNumber);
    }

    public async Task<CrmServiceLocationDetailDto> UpdateServiceLocationAsync(
        long customerId,
        long serviceLocationId,
        CrmServiceLocationUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var createDto = ToLocationCreateDto(dto);
        EnsureLocationContact(createDto);
        EnsureDistinctTagIds(dto.TagIds);

        if (!Enum.IsDefined(dto.ServiceLocationType))
        {
            throw new ArgumentException("ServiceLocationType must be between 1 and 5.");
        }

        var customer = await FindEntityAsync(customerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer '{customerId}' was not found.");

        var location = await _context.CrmServiceLocations
            .FirstOrDefaultAsync(
                e => e.Id == serviceLocationId && e.CustomerId == customer.Id,
                cancellationToken)
            ?? throw new KeyNotFoundException($"Service location '{serviceLocationId}' was not found.");

        ApplyServiceLocationFields(location, dto);
        _auditHelper.StampForUpdate(location);

        await SaveGraphAsync(async ct =>
        {
            await ReplaceServiceLocationTagsAsync(location.Id, dto.TagIds, ct);
            await SyncLocationContactAsync(location, createDto, ct);
            await _context.SaveChangesAsync(ct);
        }, cancellationToken);

        return (await _readRepository.GetServiceLocationByIdAsync(customer.Id, location.Id, cancellationToken))!;
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

    public async Task<CrmDefaultCustomerDto> UpsertDefaultCustomerAsync(
        CrmDefaultCustomerUpsertDto dto,
        CancellationToken cancellationToken = default)
    {
        var scope = new CrmDefaultCustomer();
        _auditHelper.StampForCreate(scope);

        var entity = await _context.CrmDefaultCustomers
            .FirstOrDefaultAsync(
                d => d.TenantId == scope.TenantId && d.CompanyId == scope.CompanyId,
                cancellationToken);

        if (entity is null)
        {
            ApplyDefaultCustomer(scope, dto);
            await _context.CrmDefaultCustomers.AddAsync(scope, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return MapDefaultCustomer(scope);
        }

        ApplyDefaultCustomer(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapDefaultCustomer(entity);
    }

    public async Task<CrmDefaultServiceLocationDto> UpsertDefaultServiceLocationAsync(
        CrmDefaultServiceLocationUpsertDto dto,
        CancellationToken cancellationToken = default)
    {
        var scope = new CrmDefaultServiceLocation();
        _auditHelper.StampForCreate(scope);

        var entity = await _context.CrmDefaultServiceLocations
            .FirstOrDefaultAsync(
                d => d.TenantId == scope.TenantId && d.CompanyId == scope.CompanyId,
                cancellationToken);

        if (entity is null)
        {
            ApplyDefaultServiceLocation(scope, dto);
            await _context.CrmDefaultServiceLocations.AddAsync(scope, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return MapDefaultServiceLocation(scope);
        }

        ApplyDefaultServiceLocation(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapDefaultServiceLocation(entity);
    }

    private static void ApplyDefaultCustomer(CrmDefaultCustomer entity, CrmDefaultCustomerUpsertDto dto)
    {
        entity.DefaultPaymentTermId = dto.DefaultPaymentTermId;
        entity.DefaultMaterialPricingMatrixId = dto.DefaultMaterialPricingMatrixId;
        entity.DefaultLaborPricingMatrixId = dto.DefaultLaborPricingMatrixId;
        entity.DefaultOtherPricingMatrixId = dto.DefaultOtherPricingMatrixId;
        entity.DefaultPORequired = dto.DefaultPORequired;
        entity.TaxExempt = dto.TaxExempt;
    }

    private static void ApplyDefaultServiceLocation(
        CrmDefaultServiceLocation entity,
        CrmDefaultServiceLocationUpsertDto dto)
    {
        entity.DefaultLaborPricingMatrixId = dto.DefaultLaborPricingMatrixId;
        entity.DefaultMaterialPricingMatrixId = dto.DefaultMaterialPricingMatrixId;
        entity.DefaultOtherPricingMatrixId = dto.DefaultOtherPricingMatrixId;
        entity.DefaultPaymentMethodId = dto.DefaultPaymentMethodId;
        entity.EmailAllowed = dto.EmailAllowed;
        entity.EstimateEmailTemplateId = dto.EstimateEmailTemplateId;
        entity.EstimateSmsTemplateId = dto.EstimateSmsTemplateId;
        entity.InvoiceEmailTemplateId = dto.InvoiceEmailTemplateId;
        entity.InvoiceSmsTemplateId = dto.InvoiceSmsTemplateId;
        entity.SmsAllowed = dto.SmsAllowed;
        entity.TaxExempt = dto.TaxExempt;
    }

    private static CrmDefaultCustomerDto MapDefaultCustomer(CrmDefaultCustomer entity) =>
        new(
            entity.Id,
            entity.DefaultPaymentTermId,
            entity.DefaultMaterialPricingMatrixId,
            entity.DefaultLaborPricingMatrixId,
            entity.DefaultOtherPricingMatrixId,
            entity.DefaultPORequired,
            entity.TaxExempt);

    private static CrmDefaultServiceLocationDto MapDefaultServiceLocation(CrmDefaultServiceLocation entity) =>
        new(
            entity.Id,
            entity.DefaultLaborPricingMatrixId,
            entity.DefaultMaterialPricingMatrixId,
            entity.DefaultOtherPricingMatrixId,
            entity.DefaultPaymentMethodId,
            entity.EmailAllowed,
            entity.EstimateEmailTemplateId,
            entity.EstimateSmsTemplateId,
            entity.InvoiceEmailTemplateId,
            entity.InvoiceSmsTemplateId,
            entity.SmsAllowed,
            entity.TaxExempt);

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
        entity.DefaultPaymentTermId = dto.DefaultPaymentTermId ?? defaults?.DefaultPaymentTermId;
        entity.DefaultPORequired = dto.DefaultPORequired ?? defaults?.DefaultPORequired ?? false;
        entity.TaxExempt = dto.TaxExempt ?? defaults?.TaxExempt ?? false;
    }

    private CrmContact? AddPrimaryContact(CrmCustomer customer, CrmCustomerCreateDto dto)
    {
        if (!HasPrimaryContactInput(dto))
        {
            return null;
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
        return contact;
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
            Break1Id = dto.Break1Id,
            Break2Id = dto.Break2Id,
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
        return location;
    }

    private static void ApplyServiceLocationFields(CrmServiceLocation location, CrmServiceLocationUpdateDto dto)
    {
        location.Name = EmptyIfBlank(dto.Name);
        location.DisplayName = EmptyIfBlank(dto.DisplayName);
        location.ServiceLocationType = dto.ServiceLocationType;
        location.CustomerType = dto.CustomerType;
        location.Break1Id = dto.Break1Id;
        location.Break2Id = dto.Break2Id;
        location.AddressLine1 = TrimOrNull(dto.AddressLine1);
        location.AddressLine2 = TrimOrNull(dto.AddressLine2);
        location.AddressLine3 = TrimOrNull(dto.AddressLine3);
        location.AddressLine4 = TrimOrNull(dto.AddressLine4);
        location.City = TrimOrNull(dto.City);
        location.State = TrimOrNull(dto.State);
        location.County = TrimOrNull(dto.County);
        location.Country = TrimOrNull(dto.Country);
        location.PostalCode = TrimOrNull(dto.PostalCode);
        location.FormattedAddress = TrimOrNull(dto.FormattedAddress);
        location.Latitude = dto.Latitude;
        location.Longitude = dto.Longitude;
        location.PlaceId = TrimOrNull(dto.PlaceId);
        location.TaxExempt = dto.TaxExempt ?? false;
        location.IsActive = dto.IsActive;
    }

    private static CrmServiceLocationCreateDto ToLocationCreateDto(CrmServiceLocationUpdateDto dto) =>
        new(
            dto.ServiceLocationType,
            dto.Name,
            dto.DisplayName,
            dto.AddressLine1,
            dto.AddressLine2,
            dto.AddressLine3,
            dto.AddressLine4,
            dto.City,
            dto.State,
            dto.County,
            dto.Country,
            dto.PostalCode,
            dto.FormattedAddress,
            dto.Latitude,
            dto.Longitude,
            dto.PlaceId,
            dto.TaxExempt,
            dto.PrimaryContactName,
            dto.PrimaryContactEmail,
            dto.PrimaryContactPhone,
            dto.CanReceiveEstimates,
            dto.CanReceiveInvoices,
            dto.CanReceiveAppointments,
            dto.TagIds,
            dto.CustomerType);

    private async Task SyncLocationContactAsync(
        CrmServiceLocation location,
        CrmServiceLocationCreateDto dto,
        CancellationToken cancellationToken)
    {
        var contact = await _context.CrmContacts
            .FirstOrDefaultAsync(
                c => c.IsDefaultContact && c.ServiceLocationId == location.Id && c.CustomerId == null,
                cancellationToken);

        if (contact is null)
        {
            var created = AddLocationPrimaryContact(location, dto);
            if (created is null)
            {
                return;
            }

            await _context.SaveChangesAsync(cancellationToken);
            AddCommunication(created, ContactCommunicationType.Email, dto.PrimaryContactEmail);
            AddCommunication(created, ContactCommunicationType.Phone, dto.PrimaryContactPhone);
            return;
        }

        contact.DisplayName = string.IsNullOrWhiteSpace(dto.PrimaryContactName)
            ? string.Empty
            : dto.PrimaryContactName.Trim();
        contact.CanReceiveEstimates = dto.CanReceiveEstimates ?? false;
        contact.CanReceiveInvoices = dto.CanReceiveInvoices ?? false;
        contact.CanReceiveAppointments = dto.CanReceiveAppointments ?? true;
        _auditHelper.StampForUpdate(contact);

        await ReplacePrimaryCommunicationAsync(contact, ContactCommunicationType.Email, dto.PrimaryContactEmail, cancellationToken);
        await ReplacePrimaryCommunicationAsync(contact, ContactCommunicationType.Phone, dto.PrimaryContactPhone, cancellationToken);
    }

    private async Task ReplacePrimaryCommunicationAsync(
        CrmContact contact,
        ContactCommunicationType type,
        string? value,
        CancellationToken cancellationToken)
    {
        var typeId = (short)type;
        var rows = await _context.CrmContactCommunications
            .Where(c => c.ContactId == contact.Id && c.CommunicationTypeId == typeId)
            .ToListAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(value))
        {
            foreach (var row in rows.Where(r => r.IsPrimary || r.IsActive))
            {
                row.IsPrimary = false;
                row.IsActive = false;
                _auditHelper.StampForUpdate(row);
            }

            return;
        }

        var target = rows.FirstOrDefault(r => r.IsPrimary) ?? rows.FirstOrDefault();
        if (target is null)
        {
            AddCommunication(contact, type, value);
            return;
        }

        target.Value = value.Trim();
        target.IsPrimary = true;
        target.IsActive = true;
        _auditHelper.StampForUpdate(target);

        foreach (var other in rows.Where(r => !ReferenceEquals(r, target) && r.IsPrimary))
        {
            other.IsPrimary = false;
            _auditHelper.StampForUpdate(other);
        }
    }

    private async Task ReplaceServiceLocationTagsAsync(
        long locationId,
        IReadOnlyList<long>? tagIds,
        CancellationToken cancellationToken)
    {
        var entityTypeId = (int)CrmTaggedEntityType.ServiceLocation;
        var existing = await _context.CrmEntityTags
            .Where(t => t.EntityTypeId == entityTypeId && t.EntityId == locationId)
            .ToListAsync(cancellationToken);

        if (existing.Count > 0)
        {
            _context.CrmEntityTags.RemoveRange(existing);
        }

        AddEntityTags(locationId, CrmTaggedEntityType.ServiceLocation, tagIds);
    }

    private CrmContact? AddLocationPrimaryContact(CrmServiceLocation location, CrmServiceLocationCreateDto dto)
    {
        if (!HasLocationContactInput(dto))
        {
            return null;
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
        return contact;
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

    private async Task SaveGraphAsync(
        Func<CancellationToken, Task> persistDependents,
        CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.ExecuteInTransactionAsync(persistDependents, cancellationToken);
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
