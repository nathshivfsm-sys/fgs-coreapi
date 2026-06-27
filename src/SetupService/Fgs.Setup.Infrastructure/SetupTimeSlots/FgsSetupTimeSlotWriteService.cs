using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupTimeSlots;

public sealed class FgsSetupTimeSlotWriteService : IFgsSetupTimeSlotWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupTimeSlotWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupTimeSlotDetailDto> CreateAsync(
        FgsSetupTimeSlotCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTimeSlot
        {
            FgsSetupZoneId = dto.FgsSetupZoneId,
            Code = NormalizeCode(dto.Code),
            Name = dto.Name.Trim(),
            BeginTime = dto.BeginTime,
            EndTime = dto.EndTime,
            MarkTechArrivedLateAfter = dto.MarkTechArrivedLateAfter,
            MarkWorkOrderDelayedCompletionAfter = dto.MarkWorkOrderDelayedCompletionAfter,
            IsMobileVisible = dto.IsMobileVisible,
            IsCustomerPortalVisible = dto.IsCustomerPortalVisible
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTimeSlots.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTimeSlotDetailDto> UpdateAsync(
        long id,
        FgsSetupTimeSlotUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Time Slot '{id}' was not found.");

        entity.FgsSetupZoneId = dto.FgsSetupZoneId;
        entity.Code = NormalizeCode(dto.Code);
        entity.Name = dto.Name.Trim();
        entity.BeginTime = dto.BeginTime;
        entity.EndTime = dto.EndTime;
        entity.MarkTechArrivedLateAfter = dto.MarkTechArrivedLateAfter;
        entity.MarkWorkOrderDelayedCompletionAfter = dto.MarkWorkOrderDelayedCompletionAfter;
        entity.IsMobileVisible = dto.IsMobileVisible;
        entity.IsCustomerPortalVisible = dto.IsCustomerPortalVisible;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTimeSlotDetailDto> PatchAsync(
        long id,
        FgsSetupTimeSlotPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Time Slot '{id}' was not found.");

        if (dto.FgsSetupZoneId.HasValue)
        {
            entity.FgsSetupZoneId = dto.FgsSetupZoneId.Value;
        }
        if (dto.Code is not null)
        {
            entity.Code = NormalizeCode(dto.Code); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.BeginTime.HasValue)
        {
            entity.BeginTime = dto.BeginTime.Value;
        }
        if (dto.EndTime.HasValue)
        {
            entity.EndTime = dto.EndTime.Value;
        }
        if (dto.MarkTechArrivedLateAfter.HasValue)
        {
            entity.MarkTechArrivedLateAfter = dto.MarkTechArrivedLateAfter.Value;
        }
        if (dto.MarkWorkOrderDelayedCompletionAfter.HasValue)
        {
            entity.MarkWorkOrderDelayedCompletionAfter = dto.MarkWorkOrderDelayedCompletionAfter.Value;
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

    public async Task<FgsSetupTimeSlotDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Time Slot '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTimeSlot?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTimeSlots.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A time slot with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupTimeSlotDetailDto MapToDetail(FgsSetupTimeSlot entity) =>
        new(
            entity.Id,
            entity.FgsSetupZoneId,
            entity.Code,
            entity.Name,
            entity.BeginTime,
            entity.EndTime,
            entity.MarkTechArrivedLateAfter,
            entity.MarkWorkOrderDelayedCompletionAfter,
            entity.IsMobileVisible,
            entity.IsCustomerPortalVisible,
            entity.IsActive);
}
