using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.MultiTenancy.Persistence;

/// <summary>
/// Next display order for a tenant/company sequence, including inactive rows.
/// </summary>
public static class DisplayOrderSequence
{
    public static async Task<short> NextAsync<T>(
        IQueryable<T> source,
        Expression<Func<T, short>> displayOrder,
        CancellationToken cancellationToken = default)
    {
        using (SoftDeleteFilterAccessor.BeginSuppress())
        {
            var max = await source.MaxAsync(ToNullable(displayOrder), cancellationToken);
            if (max is null)
            {
                return 1;
            }

            if (max.Value == short.MaxValue)
            {
                throw new InvalidOperationException(
                    "The display order sequence has reached its maximum value.");
            }

            return (short)(max.Value + 1);
        }
    }

    public static async Task<int> NextAsync<T>(
        IQueryable<T> source,
        Expression<Func<T, int>> displayOrder,
        CancellationToken cancellationToken = default)
    {
        using (SoftDeleteFilterAccessor.BeginSuppress())
        {
            var max = await source.MaxAsync(ToNullable(displayOrder), cancellationToken);
            if (max is null)
            {
                return 1;
            }

            if (max.Value == int.MaxValue)
            {
                throw new InvalidOperationException(
                    "The display order sequence has reached its maximum value.");
            }

            return max.Value + 1;
        }
    }

    public static short TakeNext(ref short cursor, ref bool sequenceFull)
    {
        if (sequenceFull)
        {
            throw new InvalidOperationException(
                "The display order sequence has reached its maximum value.");
        }

        var value = cursor;
        if (value == short.MaxValue)
        {
            sequenceFull = true;
            return value;
        }

        cursor++;
        return value;
    }

    public static int TakeNext(ref int cursor, ref bool sequenceFull)
    {
        if (sequenceFull)
        {
            throw new InvalidOperationException(
                "The display order sequence has reached its maximum value.");
        }

        var value = cursor;
        if (value == int.MaxValue)
        {
            sequenceFull = true;
            return value;
        }

        cursor++;
        return value;
    }

    private static Expression<Func<T, short?>> ToNullable<T>(Expression<Func<T, short>> selector)
    {
        var converted = Expression.Convert(selector.Body, typeof(short?));
        return Expression.Lambda<Func<T, short?>>(converted, selector.Parameters);
    }

    private static Expression<Func<T, int?>> ToNullable<T>(Expression<Func<T, int>> selector)
    {
        var converted = Expression.Convert(selector.Body, typeof(int?));
        return Expression.Lambda<Func<T, int?>>(converted, selector.Parameters);
    }
}
