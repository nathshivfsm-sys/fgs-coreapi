using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplates;

public sealed class FgsTruckStockTemplateWriteService : IFgsTruckStockTemplateWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsTruckStockTemplateWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsTruckStockTemplateDetailDto> CreateAsync(
        FgsTruckStockTemplateCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsTruckStockTemplate
        {
            TemplateCode = NormalizeCode(dto.TemplateCode),
            Name = dto.Name.Trim(),
            Description = TrimOrNull(dto.Description)
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsTruckStockTemplates.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTruckStockTemplateDetailDto> UpdateAsync(
        long id,
        FgsTruckStockTemplateUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Truck stock template '{id}' was not found.");

        entity.TemplateCode = NormalizeCode(dto.TemplateCode);
        entity.Name = dto.Name.Trim();
        entity.Description = TrimOrNull(dto.Description);

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTruckStockTemplateDetailDto> PatchAsync(
        long id,
        FgsTruckStockTemplatePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Truck stock template '{id}' was not found.");

        if (dto.TemplateCode is not null)
        {
            entity.TemplateCode = NormalizeCode(dto.TemplateCode);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = TrimOrNull(dto.Description);
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsTruckStockTemplateDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Truck stock template '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsTruckStockTemplate?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsTruckStockTemplates.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A truck stock template with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsTruckStockTemplateDetailDto MapToDetail(FgsTruckStockTemplate entity) =>
        new(entity.Id, entity.TemplateCode, entity.Name, entity.Description, entity.IsActive);
}
