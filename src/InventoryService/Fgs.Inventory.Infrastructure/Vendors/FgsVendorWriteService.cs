using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.Vendors;

public sealed class FgsVendorWriteService : IFgsVendorWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsVendorWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsVendorDetailDto> CreateAsync(
        FgsVendorCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsVendor
        {
            VendorCode = NormalizeCode(dto.VendorCode),
            Name = dto.Name.Trim(),
            LegalName = TrimOrNull(dto.LegalName),
            VendorType = dto.VendorType.Trim().ToUpperInvariant(),
            VendorStatus = string.IsNullOrWhiteSpace(dto.VendorStatus)
                ? VendorStatuses.Active
                : dto.VendorStatus.Trim().ToUpperInvariant(),
            VendorAccountNumber = TrimOrNull(dto.VendorAccountNumber),
            PaymentTermId = dto.PaymentTermId,
            ContactName = TrimOrNull(dto.ContactName),
            ContactTitle = TrimOrNull(dto.ContactTitle),
            Email = TrimOrNull(dto.Email),
            PurchaseOrderEmail = TrimOrNull(dto.PurchaseOrderEmail),
            PhoneNumber = TrimOrNull(dto.PhoneNumber),
            MobileNumber = TrimOrNull(dto.MobileNumber),
            FaxNumber = TrimOrNull(dto.FaxNumber),
            Website = TrimOrNull(dto.Website),
            Address1 = TrimOrNull(dto.Address1),
            Address2 = TrimOrNull(dto.Address2),
            City = TrimOrNull(dto.City),
            StateProvince = TrimOrNull(dto.StateProvince),
            PostalCode = TrimOrNull(dto.PostalCode),
            Country = TrimOrNull(dto.Country),
            TaxIdNumber = TrimOrNull(dto.TaxIdNumber),
            LicenseNumber = TrimOrNull(dto.LicenseNumber),
            InsurancePolicyNumber = TrimOrNull(dto.InsurancePolicyNumber),
            Notes = TrimOrNull(dto.Notes),
            Is1099Eligible = dto.Is1099Eligible
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsVendors.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVendorDetailDto> UpdateAsync(
        long id,
        FgsVendorUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vendor '{id}' was not found.");

        ApplyMutableFields(entity, dto);

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsVendorDetailDto> PatchAsync(
        long id,
        FgsVendorPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vendor '{id}' was not found.");

        if (dto.VendorCode is not null)
        {
            entity.VendorCode = NormalizeCode(dto.VendorCode);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.LegalName is not null)
        {
            entity.LegalName = TrimOrNull(dto.LegalName);
        }

        if (dto.VendorType is not null)
        {
            entity.VendorType = dto.VendorType.Trim().ToUpperInvariant();
        }

        if (dto.VendorStatus is not null)
        {
            entity.VendorStatus = dto.VendorStatus.Trim().ToUpperInvariant();
        }

        if (dto.VendorAccountNumber is not null)
        {
            entity.VendorAccountNumber = TrimOrNull(dto.VendorAccountNumber);
        }

        if (dto.PaymentTermId.HasValue)
        {
            entity.PaymentTermId = dto.PaymentTermId.Value;
        }

        if (dto.ContactName is not null)
        {
            entity.ContactName = TrimOrNull(dto.ContactName);
        }

        if (dto.ContactTitle is not null)
        {
            entity.ContactTitle = TrimOrNull(dto.ContactTitle);
        }

        if (dto.Email is not null)
        {
            entity.Email = TrimOrNull(dto.Email);
        }

        if (dto.PurchaseOrderEmail is not null)
        {
            entity.PurchaseOrderEmail = TrimOrNull(dto.PurchaseOrderEmail);
        }

        if (dto.PhoneNumber is not null)
        {
            entity.PhoneNumber = TrimOrNull(dto.PhoneNumber);
        }

        if (dto.MobileNumber is not null)
        {
            entity.MobileNumber = TrimOrNull(dto.MobileNumber);
        }

        if (dto.FaxNumber is not null)
        {
            entity.FaxNumber = TrimOrNull(dto.FaxNumber);
        }

        if (dto.Website is not null)
        {
            entity.Website = TrimOrNull(dto.Website);
        }

        if (dto.Address1 is not null)
        {
            entity.Address1 = TrimOrNull(dto.Address1);
        }

        if (dto.Address2 is not null)
        {
            entity.Address2 = TrimOrNull(dto.Address2);
        }

        if (dto.City is not null)
        {
            entity.City = TrimOrNull(dto.City);
        }

        if (dto.StateProvince is not null)
        {
            entity.StateProvince = TrimOrNull(dto.StateProvince);
        }

        if (dto.PostalCode is not null)
        {
            entity.PostalCode = TrimOrNull(dto.PostalCode);
        }

        if (dto.Country is not null)
        {
            entity.Country = TrimOrNull(dto.Country);
        }

        if (dto.TaxIdNumber is not null)
        {
            entity.TaxIdNumber = TrimOrNull(dto.TaxIdNumber);
        }

        if (dto.LicenseNumber is not null)
        {
            entity.LicenseNumber = TrimOrNull(dto.LicenseNumber);
        }

        if (dto.InsurancePolicyNumber is not null)
        {
            entity.InsurancePolicyNumber = TrimOrNull(dto.InsurancePolicyNumber);
        }

        if (dto.Notes is not null)
        {
            entity.Notes = TrimOrNull(dto.Notes);
        }

        if (dto.Is1099Eligible.HasValue)
        {
            entity.Is1099Eligible = dto.Is1099Eligible.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    private async Task<FgsVendor?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsVendors.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A vendor with the same code already exists.", ex);
        }
    }

    private static void ApplyMutableFields(FgsVendor entity, FgsVendorUpdateDto dto)
    {
        entity.VendorCode = NormalizeCode(dto.VendorCode);
        entity.Name = dto.Name.Trim();
        entity.LegalName = TrimOrNull(dto.LegalName);
        entity.VendorType = dto.VendorType.Trim().ToUpperInvariant();
        entity.VendorStatus = dto.VendorStatus.Trim().ToUpperInvariant();
        entity.VendorAccountNumber = TrimOrNull(dto.VendorAccountNumber);
        entity.PaymentTermId = dto.PaymentTermId;
        entity.ContactName = TrimOrNull(dto.ContactName);
        entity.ContactTitle = TrimOrNull(dto.ContactTitle);
        entity.Email = TrimOrNull(dto.Email);
        entity.PurchaseOrderEmail = TrimOrNull(dto.PurchaseOrderEmail);
        entity.PhoneNumber = TrimOrNull(dto.PhoneNumber);
        entity.MobileNumber = TrimOrNull(dto.MobileNumber);
        entity.FaxNumber = TrimOrNull(dto.FaxNumber);
        entity.Website = TrimOrNull(dto.Website);
        entity.Address1 = TrimOrNull(dto.Address1);
        entity.Address2 = TrimOrNull(dto.Address2);
        entity.City = TrimOrNull(dto.City);
        entity.StateProvince = TrimOrNull(dto.StateProvince);
        entity.PostalCode = TrimOrNull(dto.PostalCode);
        entity.Country = TrimOrNull(dto.Country);
        entity.TaxIdNumber = TrimOrNull(dto.TaxIdNumber);
        entity.LicenseNumber = TrimOrNull(dto.LicenseNumber);
        entity.InsurancePolicyNumber = TrimOrNull(dto.InsurancePolicyNumber);
        entity.Notes = TrimOrNull(dto.Notes);
        entity.Is1099Eligible = dto.Is1099Eligible;
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsVendorDetailDto MapToDetail(FgsVendor entity) =>
        new(
            entity.Id,
            entity.VendorCode,
            entity.Name,
            entity.LegalName,
            entity.VendorType,
            entity.VendorStatus,
            entity.VendorAccountNumber,
            entity.PaymentTermId,
            entity.ContactName,
            entity.ContactTitle,
            entity.Email,
            entity.PurchaseOrderEmail,
            entity.PhoneNumber,
            entity.MobileNumber,
            entity.FaxNumber,
            entity.Website,
            entity.Address1,
            entity.Address2,
            entity.City,
            entity.StateProvince,
            entity.PostalCode,
            entity.Country,
            entity.TaxIdNumber,
            entity.LicenseNumber,
            entity.InsurancePolicyNumber,
            entity.Notes,
            entity.Is1099Eligible,
            entity.IsActive);
}
