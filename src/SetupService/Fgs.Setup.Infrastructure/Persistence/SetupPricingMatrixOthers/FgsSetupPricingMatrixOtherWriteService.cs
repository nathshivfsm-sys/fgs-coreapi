using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixOthers;

public sealed class FgsSetupPricingMatrixOtherWriteService(FgsSetupDbContext _context, IUnitOfWork _unitOfWork, SetupEntityAuditHelper _auditHelper) : IFgsSetupPricingMatrixOtherWriteService
{
    public async Task<FgsSetupPricingMatrixOtherDetailDto> CreateAsync(FgsSetupPricingMatrixOtherCreateDto dto, CancellationToken cancellationToken=default)
    {
        var entity=new FgsSetupPricingMatrixOther
        {
            PricingMatrixId = dto.PricingMatrixId,
            CategoryCode = dto.CategoryCode.Trim().ToUpperInvariant(),
            Name = dto.Name.Trim(),
            AdjustmentValue = dto.AdjustmentValue,
            DiscountPercent = dto.DiscountPercent
        };

        _auditHelper.StampForCreate(entity); await _context.FgsSetupPricingMatrixOthers.AddAsync(entity,cancellationToken); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixOtherDetailDto> UpdateAsync(long id,FgsSetupPricingMatrixOtherUpdateDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Other '"+id+"' was not found.");
        entity.PricingMatrixId = dto.PricingMatrixId;
        entity.CategoryCode = dto.CategoryCode.Trim().ToUpperInvariant();
        entity.Name = dto.Name.Trim();
        entity.AdjustmentValue = dto.AdjustmentValue;
        entity.DiscountPercent = dto.DiscountPercent;

        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixOtherDetailDto> PatchAsync(long id,FgsSetupPricingMatrixOtherPatchDto dto,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Other '"+id+"' was not found.");
        if (dto.PricingMatrixId.HasValue) entity.PricingMatrixId = dto.PricingMatrixId.Value;
        if (dto.CategoryCode is not null) entity.CategoryCode = dto.CategoryCode.Trim().ToUpperInvariant();
        if (dto.Name is not null) entity.Name = dto.Name.Trim();
        if (dto.AdjustmentValue.HasValue) entity.AdjustmentValue = dto.AdjustmentValue.Value;
        if (dto.DiscountPercent.HasValue) entity.DiscountPercent = dto.DiscountPercent.Value;
        if(dto.IsActive.HasValue) entity.IsActive=dto.IsActive.Value;

        _auditHelper.StampForUpdate(entity); await SaveAsync(cancellationToken); return Map(entity);
    }
    public async Task<FgsSetupPricingMatrixOtherDetailDto> DeleteAsync(long id,CancellationToken cancellationToken=default)
    {
        var entity=await FindAsync(id,cancellationToken) ?? throw new KeyNotFoundException("Pricing Matrix Other '"+id+"' was not found.");
        if(entity.IsActive){entity.IsActive=false;_auditHelper.StampForUpdate(entity);await SaveAsync(cancellationToken);} return Map(entity);
    }
    private Task<FgsSetupPricingMatrixOther?> FindAsync(long id,CancellationToken ct)=>_context.FgsSetupPricingMatrixOthers.FirstOrDefaultAsync(x=>x.Id==id,ct);

    private async Task SaveAsync(CancellationToken ct)
    {
        try{await _unitOfWork.SaveChangesAsync(ct);}
        catch(DbUpdateException ex) when(ex.InnerException?.Message.Contains("23505",StringComparison.Ordinal)==true || ex.InnerException?.Message.Contains("unique",StringComparison.OrdinalIgnoreCase)==true)
        {throw new InvalidOperationException("A pricing matrix other with the same key already exists.",ex);}
    }
    private static FgsSetupPricingMatrixOtherDetailDto Map(FgsSetupPricingMatrixOther entity)=>new(entity.Id, entity.PricingMatrixId, entity.CategoryCode, entity.Name, entity.AdjustmentValue, entity.DiscountPercent, entity.IsActive);
}
