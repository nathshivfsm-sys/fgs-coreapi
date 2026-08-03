using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.Common;

public static class InventoryChildCollectionSync
{
    public static void Sync<TEntity, TDto>(
        DbContext context,
        ICollection<TEntity> existing,
        IReadOnlyList<TDto> dtos,
        Func<TDto, long?> getId,
        Func<TDto, TEntity> createNew,
        Action<TEntity, TDto, bool> apply,
        Action<TEntity> stampCreate,
        Action<TEntity> stampUpdate,
        string missingChildMessage)
        where TEntity : class
    {
        var existingById = existing
            .Where(e => GetEntityId(e) != 0)
            .ToDictionary(GetEntityId);
        var keep = new HashSet<TEntity>();

        foreach (var dto in dtos)
        {
            var dtoId = getId(dto);
            TEntity entity;
            var isNew = false;

            if (dtoId is > 0 && existingById.TryGetValue(dtoId.Value, out var found))
            {
                entity = found;
                stampUpdate(entity);
            }
            else if (dtoId is > 0)
            {
                throw new KeyNotFoundException(string.Format(missingChildMessage, dtoId.Value));
            }
            else
            {
                entity = createNew(dto);
                stampCreate(entity);
                existing.Add(entity);
                isNew = true;
            }

            apply(entity, dto, isNew);
            keep.Add(entity);
        }

        var toRemove = existing.Where(e => !keep.Contains(e)).ToList();
        if (toRemove.Count > 0)
        {
            context.Set<TEntity>().RemoveRange(toRemove);
        }
    }

    private static long GetEntityId<TEntity>(TEntity entity)
        where TEntity : class
    {
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty?.PropertyType != typeof(long))
        {
            return 0;
        }

        return idProperty.GetValue(entity) is long id ? id : 0;
    }
}
