using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Asset.Infrastructure.AssetTypes;

public sealed class FgsAssetTypeWriteService : IFgsAssetTypeWriteService
{
    private readonly FgsAssetDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetTypeWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper)
    { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }

    public async Task<FgsAssetTypeDetailDto> CreateAsync(FgsAssetTypeCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new FgsAssetType { Code = dto.Code.Trim().ToUpperInvariant(), Name = dto.Name.Trim(), Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim() };
        _auditHelper.StampForCreate(entity);
        await _context.FgsAssetTypes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<FgsAssetTypeDetailDto> UpdateAsync(long id, FgsAssetTypeUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Type '{id}' was not found.");
        entity.Code = dto.Code.Trim().ToUpperInvariant();
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task<FgsAssetTypeDetailDto> PatchAsync(long id, FgsAssetTypePatchDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Type '{id}' was not found.");
        if (dto.Code is not null) entity.Code = dto.Code.Trim().ToUpperInvariant();
        if (dto.Name is not null) entity.Name = dto.Name.Trim();
        if (dto.Description is not null) entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    private async Task<FgsAssetType?> FindAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsAssetTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try { await _unitOfWork.SaveChangesAsync(cancellationToken); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true || ex.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true)
        { throw new InvalidOperationException("A asset type with the same code already exists.", ex); }
    }

    private static FgsAssetTypeDetailDto Map(FgsAssetType entity) => new(entity.Id, entity.Code, entity.Name, entity.Description, entity.IsActive);
}
