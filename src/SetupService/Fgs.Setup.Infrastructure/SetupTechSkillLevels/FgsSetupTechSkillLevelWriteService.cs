using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupTechSkillLevels;

public sealed class FgsSetupTechSkillLevelWriteService : IFgsSetupTechSkillLevelWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupTechSkillLevelWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupTechSkillLevelDetailDto> CreateAsync(
        FgsSetupTechSkillLevelCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTechSkillLevel
        {
            Code = NormalizeCode(dto.Code), Name = dto.Name.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(), SortOrder = dto.SortOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTechSkillLevels.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTechSkillLevelDetailDto> UpdateAsync(
        long id,
        FgsSetupTechSkillLevelUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tech Skill Level '{id}' was not found.");

        entity.Code = NormalizeCode(dto.Code);
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.SortOrder = dto.SortOrder ?? entity.SortOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupTechSkillLevelDetailDto> PatchAsync(
        long id,
        FgsSetupTechSkillLevelPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tech Skill Level '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = NormalizeCode(dto.Code);;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();;
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

    public async Task<FgsSetupTechSkillLevelDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tech Skill Level '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTechSkillLevel?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTechSkillLevels.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A tech skill level with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupTechSkillLevelDetailDto MapToDetail(FgsSetupTechSkillLevel entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.Code,
            entity.Name,
            entity.Description,
            entity.SortOrder,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
