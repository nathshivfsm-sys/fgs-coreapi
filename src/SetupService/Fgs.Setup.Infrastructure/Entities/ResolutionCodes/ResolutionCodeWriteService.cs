using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.ResolutionCodes;

public sealed class ResolutionCodeWriteService : IResolutionCodeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public ResolutionCodeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<ResolutionCodeDetailDto> CreateAsync(
        ResolutionCodeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsResolutionCode
        {
            GloResolutionTypeId = dto.GloResolutionTypeId,
            ResolutionCode = NormalizeCode(dto.ResolutionCode),
            ResolutionName = dto.ResolutionName.Trim(),
            IsMobileVisible = dto.IsMobileVisible
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsResolutionCodes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<ResolutionCodeDetailDto> UpdateAsync(
        long id,
        ResolutionCodeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Resolution Code '{id}' was not found.");

        entity.GloResolutionTypeId = dto.GloResolutionTypeId;
        entity.ResolutionCode = NormalizeCode(dto.ResolutionCode);
        entity.ResolutionName = dto.ResolutionName.Trim();
        entity.IsMobileVisible = dto.IsMobileVisible;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<ResolutionCodeDetailDto> PatchAsync(
        long id,
        ResolutionCodePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Resolution Code '{id}' was not found.");

        if (dto.GloResolutionTypeId.HasValue)
        {
            entity.GloResolutionTypeId = dto.GloResolutionTypeId.Value;
        }
        if (dto.ResolutionCode is not null)
        {
            entity.ResolutionCode = NormalizeCode(dto.ResolutionCode); ;
        }
        if (dto.ResolutionName is not null)
        {
            entity.ResolutionName = dto.ResolutionName.Trim(); ;
        }
        if (dto.IsMobileVisible.HasValue)
        {
            entity.IsMobileVisible = dto.IsMobileVisible.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<ResolutionCodeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Resolution Code '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsResolutionCode?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsResolutionCodes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A resolution code with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static ResolutionCodeDetailDto MapToDetail(FgsResolutionCode entity) =>
        new(
            entity.Id,
            entity.GloResolutionTypeId,
            entity.ResolutionCode,
            entity.ResolutionName,
            entity.IsMobileVisible,
            entity.IsActive);
}
