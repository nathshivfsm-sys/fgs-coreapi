using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.MultiTenancy.Persistence;

/// <summary>
/// Query helpers that include soft-deleted (<c>IsActive = false</c>) rows while keeping tenant filters.
/// </summary>
public static class SoftDeleteQueryExtensions
{
    /// <summary>
    /// Same as <see cref="EntityFrameworkQueryableExtensions.FirstOrDefaultAsync{TSource}(IQueryable{TSource}, Expression{Func{TSource, bool}}, CancellationToken)"/>
    /// but with the soft-delete filter suppressed for the duration of the query.
    /// </summary>
    public static async Task<TSource?> FirstOrDefaultIncludingInactiveAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TSource : class
    {
        using (SoftDeleteFilterAccessor.BeginSuppress())
        {
            return await source.FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
