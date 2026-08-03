using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixMaterialTiers;

public sealed class FgsSetupPricingMatrixMaterialTierWriteService(FgsSetupDbContext _context, IUnitOfWork _unitOfWork, SetupEntityAuditHelper _auditHelper) : IFgsSetupPricingMatrixMaterialTierWriteService
{
    public async Task<FgsSetupPricingMatrixMaterialTierDetailDto> CreateAsync(FgsSetupPricingMatrixMaterialTierCreateDto dto, CancellationToken cancellationToken=default)
    {
        var entity=new FgsSetupPricingMatrixMaterialTier
        {
            PricingMatrixId = dto.PricingMatrixId,
            FromCost = dto.FromCost,
            ToCost = dto.ToCost,
            AdjustmentValue = dto.AdjustmentValue
        };

        _auditHelper.StampForCreate(entity); await _context.FgsSetupPricingMatrixMaterialTiers.AddAsync(entity,cancellationToken); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixMaterialTierDetailDto> UpdateAsync(long id,FgsSetupPricingMatrixMaterialTierUpdateDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Material Tier '"+id+"' was not found.");
        entity.PricingMatrixId = dto.PricingMatrixId;
        entity.FromCost = dto.FromCost;
        entity.ToCost = dto.ToCost;
        entity.AdjustmentValue = dto.AdjustmentValue;

        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixMaterialTierDetailDto> PatchAsync(long id,FgsSetupPricingMatrixMaterialTierPatchDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Material Tier '"+id+"' was not found.");
        if (dto.PricingMatrixId.HasValue) entity.PricingMatrixId = dto.PricingMatrixId.Value;
        if (dto.FromCost.HasValue) entity.FromCost = dto.FromCost.Value;
        if (dto.ToCost.HasValue) entity.ToCost = dto.ToCost.Value;
        if (dto.AdjustmentValue.HasValue) entity.AdjustmentValue = dto.AdjustmentValue.Value;
        if(dto.IsActive.HasValue) entity.IsActive=dto.IsActive.Value;

        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixMaterialTierDetailDto> DeleteAsync(long id,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Material Tier '"+id+"' was not found.");
        if(entity.IsActive){entity.IsActive=false;_auditHelper.StampForUpdate(entity);await SaveAsync(cancellationToken);} return Map(entity);
    }
    private Task<FgsSetupPricingMatrixMaterialTier?> FindAsync(long id,CancellationToken ct)=>_context.FgsSetupPricingMatrixMaterialTiers.FirstOrDefaultAsync(x=>x.Id==id,ct);

    private async Task SaveAsync(CancellationToken ct)
    {
        try{await _unitOfWork.SaveChangesAsync(ct);}
        catch(DbUpdateException ex) when(ex.InnerException?.Message.Contains("23505",StringComparison.Ordinal)==true || ex.InnerException?.Message.Contains("unique",StringComparison.OrdinalIgnoreCase)==true)
        {throw new InvalidOperationException("A pricing matrix material tier with the same key already exists.",ex);}
    }
    private static FgsSetupPricingMatrixMaterialTierDetailDto Map(FgsSetupPricingMatrixMaterialTier entity)=>new(entity.Id, entity.PricingMatrixId, entity.FromCost, entity.ToCost, entity.AdjustmentValue, entity.IsActive);
}
