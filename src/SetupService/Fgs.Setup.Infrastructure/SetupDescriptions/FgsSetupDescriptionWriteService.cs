using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupDescriptions;

public sealed class FgsSetupDescriptionWriteService : IFgsSetupDescriptionWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupDescriptionWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupDescriptionDetailDto> CreateAsync(
        FgsSetupDescriptionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupDescription
        {
            DescriptionTypeCode = dto.DescriptionTypeCode.Trim(),
            ShortNote = string.IsNullOrWhiteSpace(dto.ShortNote) ? null : dto.ShortNote.Trim(),
            Body = dto.Body.Trim(),
            FgsSetupTechTradeId = dto.FgsSetupTechTradeId,
            SortOrder = dto.SortOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupDescriptions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupDescriptionDetailDto> UpdateAsync(
        long id,
        FgsSetupDescriptionUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Setup Description '{id}' was not found.");

        entity.DescriptionTypeCode = dto.DescriptionTypeCode.Trim();
        entity.ShortNote = string.IsNullOrWhiteSpace(dto.ShortNote) ? null : dto.ShortNote.Trim();
        entity.Body = dto.Body.Trim();
        entity.FgsSetupTechTradeId = dto.FgsSetupTechTradeId;
        entity.SortOrder = dto.SortOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupDescriptionDetailDto> PatchAsync(
        long id,
        FgsSetupDescriptionPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Setup Description '{id}' was not found.");

        if (dto.DescriptionTypeCode is not null)
        {
            entity.DescriptionTypeCode = dto.DescriptionTypeCode.Trim(); ;
        }
        if (dto.ShortNote is not null)
        {
            entity.ShortNote = string.IsNullOrWhiteSpace(dto.ShortNote) ? null : dto.ShortNote.Trim(); ;
        }
        if (dto.Body is not null)
        {
            entity.Body = dto.Body.Trim(); ;
        }
        if (dto.FgsSetupTechTradeId.HasValue)
        {
            entity.FgsSetupTechTradeId = dto.FgsSetupTechTradeId.Value;
        }
        if (dto.SortOrder.HasValue)
        {
            entity.SortOrder = dto.SortOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupDescriptionDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Setup Description '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupDescription?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupDescriptions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A setup description with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupDescriptionDetailDto MapToDetail(FgsSetupDescription entity) =>
        new(
            entity.Id,
            entity.DescriptionTypeCode,
            entity.ShortNote,
            entity.Body,
            entity.FgsSetupTechTradeId,
            entity.SortOrder,
            entity.IsActive);
}
