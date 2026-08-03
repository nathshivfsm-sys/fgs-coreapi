using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLabors;

public sealed class FgsSetupPricingMatrixLaborWriteService(FgsSetupDbContext _context, IUnitOfWork _unitOfWork, SetupEntityAuditHelper _auditHelper) : IFgsSetupPricingMatrixLaborWriteService
{
    public async Task<FgsSetupPricingMatrixLaborDetailDto> CreateAsync(FgsSetupPricingMatrixLaborCreateDto dto, CancellationToken cancellationToken=default)
    {
        var entity=new FgsSetupPricingMatrixLabor
        {
            PricingMatrixId = dto.PricingMatrixId,
            LaborRateTypeId = dto.LaborRateTypeId,
            TechSkillLevelId = dto.TechSkillLevelId,
            BaseRate = dto.BaseRate,
            OvertimeMultiplier = dto.OvertimeMultiplier,
            DoubleTimeMultiplier = dto.DoubleTimeMultiplier,
            DiscountPercent = dto.DiscountPercent
        };

        var tiered = await IsTieredAsync(dto.PricingMatrixId, cancellationToken);
        if (tiered) { entity.BaseRate=0m; entity.OvertimeMultiplier=null; entity.DoubleTimeMultiplier=null; entity.DiscountPercent=null; }
        _auditHelper.StampForCreate(entity); await _context.FgsSetupPricingMatrixLabors.AddAsync(entity,cancellationToken); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixLaborDetailDto> UpdateAsync(long id,FgsSetupPricingMatrixLaborUpdateDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Labor '"+id+"' was not found.");
        entity.PricingMatrixId = dto.PricingMatrixId;
        entity.LaborRateTypeId = dto.LaborRateTypeId;
        entity.TechSkillLevelId = dto.TechSkillLevelId;
        entity.BaseRate = dto.BaseRate;
        entity.OvertimeMultiplier = dto.OvertimeMultiplier;
        entity.DoubleTimeMultiplier = dto.DoubleTimeMultiplier;
        entity.DiscountPercent = dto.DiscountPercent;

        var tiered = await IsTieredAsync(entity.PricingMatrixId, cancellationToken);
        if (tiered) { entity.BaseRate=0m; entity.OvertimeMultiplier=null; entity.DoubleTimeMultiplier=null; entity.DiscountPercent=null; }
        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixLaborDetailDto> PatchAsync(long id,FgsSetupPricingMatrixLaborPatchDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Labor '"+id+"' was not found.");
        if (dto.PricingMatrixId.HasValue) entity.PricingMatrixId = dto.PricingMatrixId.Value;
        if (dto.LaborRateTypeId.HasValue) entity.LaborRateTypeId = dto.LaborRateTypeId.Value;
        if (dto.TechSkillLevelId.HasValue) entity.TechSkillLevelId = dto.TechSkillLevelId.Value;
        if (dto.BaseRate.HasValue) entity.BaseRate = dto.BaseRate.Value;
        if (dto.OvertimeMultiplier.HasValue) entity.OvertimeMultiplier = dto.OvertimeMultiplier.Value;
        if (dto.DoubleTimeMultiplier.HasValue) entity.DoubleTimeMultiplier = dto.DoubleTimeMultiplier.Value;
        if (dto.DiscountPercent.HasValue) entity.DiscountPercent = dto.DiscountPercent.Value;
        if(dto.IsActive.HasValue) entity.IsActive=dto.IsActive.Value;

        var tiered = await IsTieredAsync(entity.PricingMatrixId, cancellationToken);
        if (tiered) { entity.BaseRate=0m; entity.OvertimeMultiplier=null; entity.DoubleTimeMultiplier=null; entity.DiscountPercent=null; }
        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixLaborDetailDto> DeleteAsync(long id,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Labor '"+id+"' was not found.");
        if(entity.IsActive){entity.IsActive=false;_auditHelper.StampForUpdate(entity);await SaveAsync(cancellationToken);} return Map(entity);
    }
    private Task<FgsSetupPricingMatrixLabor?> FindAsync(long id,CancellationToken ct)=>_context.FgsSetupPricingMatrixLabors.FirstOrDefaultAsync(x=>x.Id==id,ct);

    private async Task<bool> IsTieredAsync(long matrixId,CancellationToken ct) =>
        await _context.FgsSetupPricingMatrices.Where(x=>x.Id==matrixId).Select(x=>x.IsLaborTierStructure).FirstOrDefaultAsync(ct);

    private async Task SaveAsync(CancellationToken ct)
    {
        try{await _unitOfWork.SaveChangesAsync(ct);}
        catch(DbUpdateException ex) when(ex.InnerException?.Message.Contains("23505",StringComparison.Ordinal)==true || ex.InnerException?.Message.Contains("unique",StringComparison.OrdinalIgnoreCase)==true)
        {throw new InvalidOperationException("A pricing matrix labor with the same key already exists.",ex);}
    }
    private static FgsSetupPricingMatrixLaborDetailDto Map(FgsSetupPricingMatrixLabor entity)=>new(entity.Id, entity.PricingMatrixId, entity.LaborRateTypeId, entity.TechSkillLevelId, entity.BaseRate, entity.OvertimeMultiplier, entity.DoubleTimeMultiplier, entity.DiscountPercent, entity.IsActive);
}
