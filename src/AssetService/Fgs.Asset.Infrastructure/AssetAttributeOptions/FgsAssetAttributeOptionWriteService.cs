using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetAttributeOptions;
public sealed class FgsAssetAttributeOptionWriteService : IFgsAssetAttributeOptionWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetAttributeOptionWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetAttributeOptionDetailDto> CreateAsync(FgsAssetAttributeOptionCreateDto dto, CancellationToken cancellationToken = default) { var entity = new FgsAssetAttributeOption { AssetAttributeId = dto.AssetAttributeId, OptionCode = dto.OptionCode.Trim().ToUpperInvariant(), OptionName = dto.OptionName.Trim(), DisplayOrder = dto.DisplayOrder }; _auditHelper.StampForCreate(entity); await _context.FgsAssetAttributeOptions.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeOptionDetailDto> UpdateAsync(long id, FgsAssetAttributeOptionUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute Option '{id}' was not found."); entity.AssetAttributeId = dto.AssetAttributeId; entity.OptionCode = dto.OptionCode.Trim().ToUpperInvariant(); entity.OptionName = dto.OptionName.Trim(); entity.DisplayOrder = dto.DisplayOrder; _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeOptionDetailDto> PatchAsync(long id, FgsAssetAttributeOptionPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute Option '{id}' was not found."); if (dto.AssetAttributeId.HasValue) entity.AssetAttributeId = dto.AssetAttributeId.Value; if (dto.OptionCode is not null) entity.OptionCode = dto.OptionCode.Trim().ToUpperInvariant(); if (dto.OptionName is not null) entity.OptionName = dto.OptionName.Trim(); if (dto.DisplayOrder.HasValue) entity.DisplayOrder = dto.DisplayOrder.Value; if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value; _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetAttributeOption?> Find(long id, CancellationToken ct) => await _context.FgsAssetAttributeOptions.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static FgsAssetAttributeOptionDetailDto Map(FgsAssetAttributeOption e) => new(e.Id, e.AssetAttributeId, e.OptionCode, e.OptionName, e.DisplayOrder, e.IsActive);
}
