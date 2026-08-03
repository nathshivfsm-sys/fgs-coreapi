using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetAttributeValues;
public sealed class FgsAssetAttributeValueWriteService : IFgsAssetAttributeValueWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetAttributeValueWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetAttributeValueDetailDto> CreateAsync(FgsAssetAttributeValueCreateDto dto, CancellationToken cancellationToken = default) { var entity = new FgsAssetAttributeValue { AssetId = dto.AssetId, AssetAttributeId = dto.AssetAttributeId, OptionId = dto.OptionId, ValueText = Trim(dto.ValueText), ValueInteger = dto.ValueInteger, ValueDecimal = dto.ValueDecimal, ValueDate = dto.ValueDate, ValueBoolean = dto.ValueBoolean }; _auditHelper.StampForCreate(entity); await _context.FgsAssetAttributeValues.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeValueDetailDto> UpdateAsync(long id, FgsAssetAttributeValueUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute Value '{id}' was not found."); Apply(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeValueDetailDto> PatchAsync(long id, FgsAssetAttributeValuePatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute Value '{id}' was not found."); Patch(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetAttributeValue?> Find(long id, CancellationToken ct) => await _context.FgsAssetAttributeValues.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static void Apply(FgsAssetAttributeValue e, FgsAssetAttributeValueUpdateDto dto) { e.AssetId = dto.AssetId; e.AssetAttributeId = dto.AssetAttributeId; e.OptionId = dto.OptionId; e.ValueText = Trim(dto.ValueText); e.ValueInteger = dto.ValueInteger; e.ValueDecimal = dto.ValueDecimal; e.ValueDate = dto.ValueDate; e.ValueBoolean = dto.ValueBoolean; }
    private static void Patch(FgsAssetAttributeValue e, FgsAssetAttributeValuePatchDto dto) { if (dto.AssetId.HasValue) e.AssetId = dto.AssetId.Value; if (dto.AssetAttributeId.HasValue) e.AssetAttributeId = dto.AssetAttributeId.Value; if (dto.OptionId.HasValue) e.OptionId = dto.OptionId; if (dto.ValueText is not null) e.ValueText = Trim(dto.ValueText); if (dto.ValueInteger.HasValue) e.ValueInteger = dto.ValueInteger; if (dto.ValueDecimal.HasValue) e.ValueDecimal = dto.ValueDecimal; if (dto.ValueDate.HasValue) e.ValueDate = dto.ValueDate; if (dto.ValueBoolean.HasValue) e.ValueBoolean = dto.ValueBoolean; }
    private static string? Trim(string? v) => string.IsNullOrWhiteSpace(v) ? null : v.Trim();
    private static FgsAssetAttributeValueDetailDto Map(FgsAssetAttributeValue e) => new(e.Id, e.AssetId, e.AssetAttributeId, e.OptionId, e.ValueText, e.ValueInteger, e.ValueDecimal, e.ValueDate, e.ValueBoolean);
}
