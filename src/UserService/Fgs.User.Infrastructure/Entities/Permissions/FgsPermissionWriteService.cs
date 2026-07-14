using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.Permissions;

public sealed class FgsPermissionWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork) : IFgsPermissionWriteService
{
    public async Task<FgsPermissionDetailDto> CreateAsync(
        FgsPermissionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsPermission
        {
            PermissionCode = NormalizePermissionCode(dto.PermissionCode),
            Module = dto.Module.Trim(),
            Resource = dto.Resource.Trim(),
            Action = dto.Action.Trim(),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

        await context.FgsPermissions.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPermissionDetailDto> UpdateAsync(
        long id,
        FgsPermissionUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Permission '{id}' was not found.");

        entity.PermissionCode = NormalizePermissionCode(dto.PermissionCode);
        entity.Module = dto.Module.Trim();
        entity.Resource = dto.Resource.Trim();
        entity.Action = dto.Action.Trim();
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsPermissionDetailDto> PatchAsync(
        long id,
        FgsPermissionPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Permission '{id}' was not found.");

        if (dto.PermissionCode is not null)
        {
            entity.PermissionCode = NormalizePermissionCode(dto.PermissionCode);
        }

        if (dto.Module is not null)
        {
            entity.Module = dto.Module.Trim();
        }

        if (dto.Resource is not null)
        {
            entity.Resource = dto.Resource.Trim();
        }

        if (dto.Action is not null)
        {
            entity.Action = dto.Action.Trim();
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description.Trim();
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsPermission?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsPermissions.FirstOrDefaultAsync(permission => permission.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A permission with the same permission code already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizePermissionCode(string permissionCode) =>
        permissionCode.Trim().ToUpperInvariant();

    private static FgsPermissionDetailDto MapToDetail(FgsPermission entity) =>
        new(
            entity.Id,
            entity.PermissionCode,
            entity.Module,
            entity.Resource,
            entity.Action,
            entity.Name,
            entity.Description,
            entity.DisplayOrder,
            entity.IsActive);
}
