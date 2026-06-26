using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Vendors;

public sealed class FgsVendorWriteService : IFgsVendorWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsVendorWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
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
            LegalName = string.IsNullOrWhiteSpace(dto.LegalName) ? null : dto.LegalName.Trim(),
            VendorType = dto.VendorType.Trim(),
            PaymentTermId = dto.PaymentTermId,
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim(),
            MobileNumber = string.IsNullOrWhiteSpace(dto.MobileNumber) ? null : dto.MobileNumber.Trim(),
            Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website.Trim(),
            TaxIdentificationNumber = string.IsNullOrWhiteSpace(dto.TaxIdentificationNumber) ? null : dto.TaxIdentificationNumber.Trim(),
            LicenseNumber = string.IsNullOrWhiteSpace(dto.LicenseNumber) ? null : dto.LicenseNumber.Trim(),
            InsurancePolicyNumber = string.IsNullOrWhiteSpace(dto.InsurancePolicyNumber) ? null : dto.InsurancePolicyNumber.Trim(),
            Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
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

        entity.VendorCode = NormalizeCode(dto.VendorCode);
        entity.Name = dto.Name.Trim();
        entity.LegalName = string.IsNullOrWhiteSpace(dto.LegalName) ? null : dto.LegalName.Trim();
        entity.VendorType = dto.VendorType.Trim();
        entity.PaymentTermId = dto.PaymentTermId;
        entity.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        entity.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim();
        entity.MobileNumber = string.IsNullOrWhiteSpace(dto.MobileNumber) ? null : dto.MobileNumber.Trim();
        entity.Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website.Trim();
        entity.TaxIdentificationNumber = string.IsNullOrWhiteSpace(dto.TaxIdentificationNumber) ? null : dto.TaxIdentificationNumber.Trim();
        entity.LicenseNumber = string.IsNullOrWhiteSpace(dto.LicenseNumber) ? null : dto.LicenseNumber.Trim();
        entity.InsurancePolicyNumber = string.IsNullOrWhiteSpace(dto.InsurancePolicyNumber) ? null : dto.InsurancePolicyNumber.Trim();
        entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
        entity.Is1099Eligible = dto.Is1099Eligible;

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
            entity.VendorCode = NormalizeCode(dto.VendorCode); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.LegalName is not null)
        {
            entity.LegalName = string.IsNullOrWhiteSpace(dto.LegalName) ? null : dto.LegalName.Trim(); ;
        }
        if (dto.VendorType is not null)
        {
            entity.VendorType = dto.VendorType.Trim(); ;
        }
        if (dto.PaymentTermId.HasValue)
        {
            entity.PaymentTermId = dto.PaymentTermId.Value;
        }
        if (dto.Email is not null)
        {
            entity.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(); ;
        }
        if (dto.PhoneNumber is not null)
        {
            entity.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim(); ;
        }
        if (dto.MobileNumber is not null)
        {
            entity.MobileNumber = string.IsNullOrWhiteSpace(dto.MobileNumber) ? null : dto.MobileNumber.Trim(); ;
        }
        if (dto.Website is not null)
        {
            entity.Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website.Trim(); ;
        }
        if (dto.TaxIdentificationNumber is not null)
        {
            entity.TaxIdentificationNumber = string.IsNullOrWhiteSpace(dto.TaxIdentificationNumber) ? null : dto.TaxIdentificationNumber.Trim(); ;
        }
        if (dto.LicenseNumber is not null)
        {
            entity.LicenseNumber = string.IsNullOrWhiteSpace(dto.LicenseNumber) ? null : dto.LicenseNumber.Trim(); ;
        }
        if (dto.InsurancePolicyNumber is not null)
        {
            entity.InsurancePolicyNumber = string.IsNullOrWhiteSpace(dto.InsurancePolicyNumber) ? null : dto.InsurancePolicyNumber.Trim(); ;
        }
        if (dto.Notes is not null)
        {
            entity.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(); ;
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

    public async Task<FgsVendorDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vendor '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

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

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsVendorDetailDto MapToDetail(FgsVendor entity) =>
        new(
            entity.Id,
            entity.VendorCode,
            entity.Name,
            entity.LegalName,
            entity.VendorType,
            entity.PaymentTermId,
            entity.Email,
            entity.PhoneNumber,
            entity.MobileNumber,
            entity.Website,
            entity.TaxIdentificationNumber,
            entity.LicenseNumber,
            entity.InsurancePolicyNumber,
            entity.Notes,
            entity.Is1099Eligible,
            entity.IsActive);
}
