using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Domain.Entities;
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

    public async Task<CrmCustomerDetailDto> CreateAsync(
        CrmCustomerCreateDto dto,
        CancellationToken cancellationToken = default)
    {
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
            DefaultMaterialPricingMatrixId = dto.DefaultMaterialPricingMatrixId,
            DefaultLaborPricingMatrixId = dto.DefaultLaborPricingMatrixId,
            DefaultOtherPricingMatrixId = dto.DefaultOtherPricingMatrixId,
            DefaultPORequired = dto.DefaultPORequired,
            TaxExempt = dto.TaxExempt,
            TaxExemptNumber = TrimOrNull(dto.TaxExemptNumber),
            CustomerAccountNumber = TrimOrNull(dto.CustomerAccountNumber),
            ExternalEntityId = TrimOrNull(dto.ExternalEntityId),
            ExternalVersion = TrimOrNull(dto.ExternalVersion)
        };

        _auditHelper.StampForCreate(entity);
        await _context.CrmCustomers.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
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

        return MapToDetail(entity);
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

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
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
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCustomerNumber(string customerNumber) =>
        customerNumber.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

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
