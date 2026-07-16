using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.Tags;

public sealed class FgsTagWriteService : IFgsTagWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsTagWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsTagDetailDto> CreateAsync(
        FgsTagCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsTag
        {
            TagCode = string.IsNullOrWhiteSpace(dto.TagCode) ? null : NormalizeCode(dto.TagCode),
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            BackgroundColor = string.IsNullOrWhiteSpace(dto.BackgroundColor) ? null : dto.BackgroundColor.Trim(),
            TextColor = string.IsNullOrWhiteSpace(dto.TextColor) ? null : dto.TextColor.Trim(),
            IconFileId = dto.IconFileId
        };

        entity.NormalizedName = dto.Name.Trim().ToUpperInvariant();
        _auditHelper.StampForCreate(entity);
        await _context.FgsTags.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTagDetailDto> UpdateAsync(
        long id,
        FgsTagUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tag '{id}' was not found.");

        entity.TagCode = string.IsNullOrWhiteSpace(dto.TagCode) ? null : NormalizeCode(dto.TagCode);
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.BackgroundColor = string.IsNullOrWhiteSpace(dto.BackgroundColor) ? null : dto.BackgroundColor.Trim();
        entity.TextColor = string.IsNullOrWhiteSpace(dto.TextColor) ? null : dto.TextColor.Trim();
        entity.IconFileId = dto.IconFileId;
        entity.NormalizedName = dto.Name.Trim().ToUpperInvariant();

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTagDetailDto> PatchAsync(
        long id,
        FgsTagPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tag '{id}' was not found.");

        if (dto.TagCode is not null)
        {
            entity.TagCode = string.IsNullOrWhiteSpace(dto.TagCode) ? null : NormalizeCode(dto.TagCode); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(); ;
        }
        if (dto.BackgroundColor is not null)
        {
            entity.BackgroundColor = string.IsNullOrWhiteSpace(dto.BackgroundColor) ? null : dto.BackgroundColor.Trim(); ;
        }
        if (dto.TextColor is not null)
        {
            entity.TextColor = string.IsNullOrWhiteSpace(dto.TextColor) ? null : dto.TextColor.Trim(); ;
        }
        if (dto.IconFileId.HasValue)
        {
            entity.IconFileId = dto.IconFileId.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTagDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tag '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsTag?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsTags.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A tag with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsTagDetailDto MapToDetail(FgsTag entity) =>
        new(
            entity.Id,
            entity.TagCode,
            entity.Name,
            entity.Description,
            entity.BackgroundColor,
            entity.TextColor,
            entity.IconFileId,
            entity.UsageCount,
            entity.IsActive);
}
