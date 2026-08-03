using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.InventorySubCategories;

public sealed class FgsInventorySubCategoryWriteService : IFgsInventorySubCategoryWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsInventorySubCategoryWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsInventorySubCategoryDetailDto> CreateAsync(
        FgsInventorySubCategoryCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsInventorySubCategory
        {
            InventoryCategoryId = dto.InventoryCategoryId,
            SubCategoryCode = NormalizeCode(dto.SubCategoryCode),
            Name = dto.Name.Trim(),
            Description = TrimOrNull(dto.Description),
            TextColor = TrimOrNull(dto.TextColor),
            BackgroundColor = TrimOrNull(dto.BackgroundColor),
            DisplayIconFileId = dto.DisplayIconFileId,
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsInventorySubCategories.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventorySubCategoryDetailDto> UpdateAsync(
        long id,
        FgsInventorySubCategoryUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory sub-category '{id}' was not found.");

        entity.InventoryCategoryId = dto.InventoryCategoryId;
        entity.SubCategoryCode = NormalizeCode(dto.SubCategoryCode);
        entity.Name = dto.Name.Trim();
        entity.Description = TrimOrNull(dto.Description);
        entity.TextColor = TrimOrNull(dto.TextColor);
        entity.BackgroundColor = TrimOrNull(dto.BackgroundColor);
        entity.DisplayIconFileId = dto.DisplayIconFileId;
        entity.DisplayOrder = dto.DisplayOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsInventorySubCategoryDetailDto> PatchAsync(
        long id,
        FgsInventorySubCategoryPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Inventory sub-category '{id}' was not found.");

        if (dto.InventoryCategoryId.HasValue) entity.InventoryCategoryId = dto.InventoryCategoryId.Value;
        if (dto.SubCategoryCode is not null) entity.SubCategoryCode = NormalizeCode(dto.SubCategoryCode);
        if (dto.Name is not null) entity.Name = dto.Name.Trim();
        if (dto.Description is not null) entity.Description = TrimOrNull(dto.Description);
        if (dto.TextColor is not null) entity.TextColor = TrimOrNull(dto.TextColor);
        if (dto.BackgroundColor is not null) entity.BackgroundColor = TrimOrNull(dto.BackgroundColor);
        if (dto.DisplayIconFileId.HasValue) entity.DisplayIconFileId = dto.DisplayIconFileId.Value;
        if (dto.DisplayOrder.HasValue) entity.DisplayOrder = dto.DisplayOrder.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsInventorySubCategory?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsInventorySubCategories.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("An inventory sub-category with the same code already exists for the category.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();
    private static string? TrimOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsInventorySubCategoryDetailDto MapToDetail(FgsInventorySubCategory entity) =>
        new(entity.Id, entity.InventoryCategoryId, entity.SubCategoryCode, entity.Name, entity.Description, entity.TextColor, entity.BackgroundColor, entity.DisplayIconFileId, entity.DisplayOrder, entity.IsSystem, entity.IsActive);
}
