using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLaborTiers;

public sealed class FgsSetupPricingMatrixLaborTierWriteService(FgsSetupDbContext _context, IUnitOfWork _unitOfWork, SetupEntityAuditHelper _auditHelper) : IFgsSetupPricingMatrixLaborTierWriteService
{
    public async Task<FgsSetupPricingMatrixLaborTierDetailDto> CreateAsync(FgsSetupPricingMatrixLaborTierCreateDto dto, CancellationToken cancellationToken=default)
    {
        var entity=new FgsSetupPricingMatrixLaborTier
        {
            PricingMatrixLaborId = dto.PricingMatrixLaborId,
            SequenceOrder = dto.SequenceOrder,
            DurationMinutes = dto.DurationMinutes,
            Rate = dto.Rate,
            TechSkillLevelId = dto.TechSkillLevelId
        };

        _auditHelper.StampForCreate(entity); await _context.FgsSetupPricingMatrixLaborTiers.AddAsync(entity,cancellationToken); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixLaborTierDetailDto> UpdateAsync(long id,FgsSetupPricingMatrixLaborTierUpdateDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Labor Tier '"+id+"' was not found.");
        entity.PricingMatrixLaborId = dto.PricingMatrixLaborId;
        entity.SequenceOrder = dto.SequenceOrder;
        entity.DurationMinutes = dto.DurationMinutes;
        entity.Rate = dto.Rate;
        entity.TechSkillLevelId = dto.TechSkillLevelId;

        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixLaborTierDetailDto> PatchAsync(long id,FgsSetupPricingMatrixLaborTierPatchDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Labor Tier '"+id+"' was not found.");
        if (dto.PricingMatrixLaborId.HasValue) entity.PricingMatrixLaborId = dto.PricingMatrixLaborId.Value;
        if (dto.SequenceOrder.HasValue) entity.SequenceOrder = dto.SequenceOrder.Value;
        if (dto.DurationMinutes.HasValue) entity.DurationMinutes = dto.DurationMinutes.Value;
        if (dto.Rate.HasValue) entity.Rate = dto.Rate.Value;
        if (dto.TechSkillLevelId.HasValue) entity.TechSkillLevelId = dto.TechSkillLevelId.Value;
        if(dto.IsActive.HasValue) entity.IsActive=dto.IsActive.Value;

        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixLaborTierDetailDto> DeleteAsync(long id,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Labor Tier '"+id+"' was not found.");
        if(entity.IsActive){entity.IsActive=false;_auditHelper.StampForUpdate(entity);await SaveAsync(cancellationToken);} return Map(entity);
    }
    private Task<FgsSetupPricingMatrixLaborTier?> FindAsync(long id,CancellationToken ct)=>_context.FgsSetupPricingMatrixLaborTiers.FirstOrDefaultAsync(x=>x.Id==id,ct);

    private async Task SaveAsync(CancellationToken ct)
    {
        try{await _unitOfWork.SaveChangesAsync(ct);}
        catch(DbUpdateException ex) when(ex.InnerException?.Message.Contains("23505",StringComparison.Ordinal)==true || ex.InnerException?.Message.Contains("unique",StringComparison.OrdinalIgnoreCase)==true)
        {throw new InvalidOperationException("A pricing matrix labor tier with the same key already exists.",ex);}
    }
    private static FgsSetupPricingMatrixLaborTierDetailDto Map(FgsSetupPricingMatrixLaborTier entity)=>new(entity.Id, entity.PricingMatrixLaborId, entity.SequenceOrder, entity.DurationMinutes, entity.Rate, entity.TechSkillLevelId, entity.IsActive);
}
