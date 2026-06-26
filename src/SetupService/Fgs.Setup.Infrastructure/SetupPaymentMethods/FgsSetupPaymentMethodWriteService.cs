using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupPaymentMethods;

public sealed class FgsSetupPaymentMethodWriteService : IFgsSetupPaymentMethodWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupPaymentMethodWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupPaymentMethodDetailDto> CreateAsync(
        FgsSetupPaymentMethodCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupPaymentMethod
        {
            DisplayName = dto.DisplayName.Trim(),
            SortOrder = dto.SortOrder,
            IsMobileVisible = dto.IsMobileVisible,
            IsCustomerPortalVisible = dto.IsCustomerPortalVisible
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupPaymentMethods.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPaymentMethodDetailDto> UpdateAsync(
        long id,
        FgsSetupPaymentMethodUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment Method '{id}' was not found.");

        entity.DisplayName = dto.DisplayName.Trim();
        entity.SortOrder = dto.SortOrder;
        entity.IsMobileVisible = dto.IsMobileVisible;
        entity.IsCustomerPortalVisible = dto.IsCustomerPortalVisible;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPaymentMethodDetailDto> PatchAsync(
        long id,
        FgsSetupPaymentMethodPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment Method '{id}' was not found.");

        if (dto.DisplayName is not null)
        {
            entity.DisplayName = dto.DisplayName.Trim(); ;
        }
        if (dto.SortOrder.HasValue)
        {
            entity.SortOrder = dto.SortOrder.Value;
        }
        if (dto.IsMobileVisible.HasValue)
        {
            entity.IsMobileVisible = dto.IsMobileVisible.Value;
        }
        if (dto.IsCustomerPortalVisible.HasValue)
        {
            entity.IsCustomerPortalVisible = dto.IsCustomerPortalVisible.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPaymentMethodDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment Method '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupPaymentMethod?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupPaymentMethods.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A payment method with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupPaymentMethodDetailDto MapToDetail(FgsSetupPaymentMethod entity) =>
        new(
            entity.Id,
            entity.DisplayName,
            entity.SortOrder,
            entity.IsMobileVisible,
            entity.IsCustomerPortalVisible,
            entity.IsActive);
}
