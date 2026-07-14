using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.DataAccesses;

public sealed class FgsDataAccessWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsDataAccessWriteService
{
    public async Task<FgsDataAccessDetailDto> CreateAsync(
        FgsDataAccessCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var now = DateTimeOffset.UtcNow;
        var actor = ResolveActor();

        var entity = new FgsDataAccess
        {
            TenantId = tenantId,
            CompanyId = companyId,
            DataAccessCode = NormalizeDataAccessCode(dto.DataAccessCode),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            IsBuiltIn = false,
            DisplayOrder = dto.DisplayOrder,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsDataAccesses.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsDataAccessDetailDto> UpdateAsync(
        long id,
        FgsDataAccessUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Data access '{id}' was not found.");

        EnsureMutable(entity);

        entity.DataAccessCode = NormalizeDataAccessCode(dto.DataAccessCode);
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        StampForUpdate(entity);

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsDataAccessDetailDto> PatchAsync(
        long id,
        FgsDataAccessPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Data access '{id}' was not found.");

        EnsureMutable(entity);

        if (dto.DataAccessCode is not null)
        {
            entity.DataAccessCode = NormalizeDataAccessCode(dto.DataAccessCode);
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

        StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsDataAccess?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsDataAccesses.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

    private static void EnsureMutable(FgsDataAccess entity)
    {
        if (entity.IsBuiltIn)
        {
            throw new InvalidOperationException("Built-in data access profiles cannot be edited or deactivated.");
        }
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A data access profile with the same data access code already exists.",
                ex);
        }
    }

    private void StampForUpdate(FgsDataAccess entity)
    {
        entity.UpdatedOn = DateTimeOffset.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }

    private string? ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString();

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeDataAccessCode(string dataAccessCode) =>
        dataAccessCode.Trim().ToUpperInvariant();

    private static FgsDataAccessDetailDto MapToDetail(FgsDataAccess entity) =>
        new(
            entity.Id,
            entity.DataAccessCode,
            entity.Name,
            entity.Description,
            entity.IsBuiltIn,
            entity.DisplayOrder,
            entity.IsActive);
}
