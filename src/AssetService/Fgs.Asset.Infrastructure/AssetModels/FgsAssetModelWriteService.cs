using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetModels;
public sealed class FgsAssetModelWriteService : IFgsAssetModelWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetModelWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetModelDetailDto> CreateAsync(FgsAssetModelCreateDto dto, CancellationToken cancellationToken = default) { var entity = new FgsAssetModel { AssetTypeId = dto.AssetTypeId, AssetManufacturerId = dto.AssetManufacturerId, ModelNumber = dto.ModelNumber.Trim(), ModelDescription = dto.ModelDescription.Trim() }; _auditHelper.StampForCreate(entity); await _context.FgsAssetModels.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetModelDetailDto> UpdateAsync(long id, FgsAssetModelUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Model '{id}' was not found."); entity.AssetTypeId = dto.AssetTypeId; entity.AssetManufacturerId = dto.AssetManufacturerId; entity.ModelNumber = dto.ModelNumber.Trim(); entity.ModelDescription = dto.ModelDescription.Trim(); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetModelDetailDto> PatchAsync(long id, FgsAssetModelPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Model '{id}' was not found."); if (dto.AssetTypeId.HasValue) entity.AssetTypeId = dto.AssetTypeId.Value; if (dto.AssetManufacturerId.HasValue) entity.AssetManufacturerId = dto.AssetManufacturerId.Value; if (dto.ModelNumber is not null) entity.ModelNumber = dto.ModelNumber.Trim(); if (dto.ModelDescription is not null) entity.ModelDescription = dto.ModelDescription.Trim(); if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value; _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetModel?> Find(long id, CancellationToken ct) => await _context.FgsAssetModels.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static FgsAssetModelDetailDto Map(FgsAssetModel e) => new(e.Id, e.AssetTypeId, e.AssetManufacturerId, e.ModelNumber, e.ModelDescription, e.IsActive);
}
