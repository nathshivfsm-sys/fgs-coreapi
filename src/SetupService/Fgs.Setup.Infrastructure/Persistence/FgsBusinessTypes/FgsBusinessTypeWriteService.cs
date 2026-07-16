using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.FgsBusinessTypes;

public sealed class FgsBusinessTypeWriteService : IFgsBusinessTypeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsBusinessTypeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsBusinessTypeDetailDto> CreateAsync(
        FgsBusinessTypeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsBusinessType
        {
            Code = NormalizeCode(dto.Code),
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            DisplayOrder = dto.DisplayOrder ?? 1
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsBusinessTypes.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsBusinessTypeDetailDto> UpdateAsync(
        long id,
        FgsBusinessTypeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Business Type '{id}' was not found.");

        entity.Code = NormalizeCode(dto.Code);
        entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.DisplayOrder = dto.DisplayOrder ?? entity.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsBusinessTypeDetailDto> PatchAsync(
        long id,
        FgsBusinessTypePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Business Type '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = NormalizeCode(dto.Code); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(); ;
        }
        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsBusinessTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Business Type '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsBusinessType?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsBusinessTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A business type with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsBusinessTypeDetailDto MapToDetail(FgsBusinessType entity) =>
        new(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.Description,
            entity.DisplayOrder,
            entity.IsActive);
}
