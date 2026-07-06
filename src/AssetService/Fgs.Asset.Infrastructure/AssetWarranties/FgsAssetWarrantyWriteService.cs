using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetWarranties;
public sealed class FgsAssetWarrantyWriteService : IFgsAssetWarrantyWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetWarrantyWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetWarrantyDetailDto> CreateAsync(FgsAssetWarrantyCreateDto dto, CancellationToken cancellationToken = default) { var entity = new FgsAssetWarranty { AssetId = dto.AssetId, WarrantyType = dto.WarrantyType.Trim().ToUpperInvariant(), WarrantyProvider = Trim(dto.WarrantyProvider), WarrantyNumber = Trim(dto.WarrantyNumber), RegistrationNumber = Trim(dto.RegistrationNumber), StartDate = dto.StartDate, EndDate = dto.EndDate, CoverageDescription = Trim(dto.CoverageDescription) }; _auditHelper.StampForCreate(entity); await _context.FgsAssetWarranties.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetWarrantyDetailDto> UpdateAsync(long id, FgsAssetWarrantyUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Warranty '{id}' was not found."); Apply(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetWarrantyDetailDto> PatchAsync(long id, FgsAssetWarrantyPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Warranty '{id}' was not found."); Patch(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetWarranty?> Find(long id, CancellationToken ct) => await _context.FgsAssetWarranties.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static void Apply(FgsAssetWarranty e, FgsAssetWarrantyUpdateDto dto) { e.AssetId = dto.AssetId; e.WarrantyType = dto.WarrantyType.Trim().ToUpperInvariant(); e.WarrantyProvider = Trim(dto.WarrantyProvider); e.WarrantyNumber = Trim(dto.WarrantyNumber); e.RegistrationNumber = Trim(dto.RegistrationNumber); e.StartDate = dto.StartDate; e.EndDate = dto.EndDate; e.CoverageDescription = Trim(dto.CoverageDescription); }
    private static void Patch(FgsAssetWarranty e, FgsAssetWarrantyPatchDto dto) { if (dto.AssetId.HasValue) e.AssetId = dto.AssetId.Value; if (dto.WarrantyType is not null) e.WarrantyType = dto.WarrantyType.Trim().ToUpperInvariant(); if (dto.WarrantyProvider is not null) e.WarrantyProvider = Trim(dto.WarrantyProvider); if (dto.WarrantyNumber is not null) e.WarrantyNumber = Trim(dto.WarrantyNumber); if (dto.RegistrationNumber is not null) e.RegistrationNumber = Trim(dto.RegistrationNumber); if (dto.StartDate.HasValue) e.StartDate = dto.StartDate.Value; if (dto.EndDate.HasValue) e.EndDate = dto.EndDate.Value; if (dto.CoverageDescription is not null) e.CoverageDescription = Trim(dto.CoverageDescription); }
    private static string? Trim(string? v) => string.IsNullOrWhiteSpace(v) ? null : v.Trim();
    private static FgsAssetWarrantyDetailDto Map(FgsAssetWarranty e) => new(e.Id, e.AssetId, e.WarrantyType, e.WarrantyProvider, e.WarrantyNumber, e.RegistrationNumber, e.StartDate, e.EndDate, e.CoverageDescription);
}
