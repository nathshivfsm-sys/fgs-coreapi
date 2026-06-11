using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Persistence.CatalogCrud;

public sealed class CatalogEntityWriteService<TDbContext> : IEntityWriteService
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntityAuditStamper _auditStamper;

    public CatalogEntityWriteService(
        TDbContext context,
        IUnitOfWork unitOfWork,
        IEntityAuditStamper auditStamper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditStamper = auditStamper;
    }

    public async Task<object> CreateAsync(
        CatalogEntityDescriptor descriptor,
        object createDto,
        CancellationToken cancellationToken = default)
    {
        var entity = CatalogEntityMapper.MapCreateDto(createDto, descriptor);
        _auditStamper.StampForCreate(entity, descriptor);

        await _context.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return CatalogEntityMapper.ToDetailDto(entity, descriptor);
    }

    public async Task<object> UpdateAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        object updateDto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(descriptor, id, cancellationToken)
            ?? throw new KeyNotFoundException($"{descriptor.EntityName} '{id}' was not found.");

        CatalogEntityMapper.MapUpdateDto(updateDto, entity, descriptor);
        _auditStamper.StampForUpdate(entity, descriptor);

        _context.Update(entity);
        await SaveChangesAsync(cancellationToken);

        return CatalogEntityMapper.ToDetailDto(entity, descriptor);
    }

    public async Task<object> PatchAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        object patchDto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(descriptor, id, cancellationToken)
            ?? throw new KeyNotFoundException($"{descriptor.EntityName} '{id}' was not found.");

        CatalogEntityMapper.MapPatchDto(patchDto, entity, descriptor);
        _auditStamper.StampForUpdate(entity, descriptor);

        _context.Update(entity);
        await SaveChangesAsync(cancellationToken);

        return CatalogEntityMapper.ToDetailDto(entity, descriptor);
    }

    public async Task DeleteAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(descriptor, id, cancellationToken)
            ?? throw new KeyNotFoundException($"{descriptor.EntityName} '{id}' was not found.");

        if (descriptor.SupportsSoftDelete)
        {
            entity.GetType().GetProperty("IsActive")?.SetValue(entity, false);
            _auditStamper.StampForUpdate(entity, descriptor);
            _context.Update(entity);
        }
        else
        {
            _context.Remove(entity);
        }

        await SaveChangesAsync(cancellationToken);
    }

    private async Task<object?> FindEntityAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        CancellationToken cancellationToken)
    {
        var parsedId = CatalogEntityMapper.ParseId(id, descriptor.KeyType)
            ?? throw new FormatException($"Invalid identifier '{id}'.");

        return descriptor.KeyType switch
        {
            CatalogEntityKeyType.Long => await _context.FindAsync(descriptor.ClrType, [parsedId], cancellationToken),
            CatalogEntityKeyType.Guid => await _context.FindAsync(descriptor.ClrType, [parsedId], cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(descriptor))
        };
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A record with the same unique values already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505") == true;
}
