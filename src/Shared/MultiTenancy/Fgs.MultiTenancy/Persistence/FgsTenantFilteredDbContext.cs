using System.Reflection;
using Fgs.Kernel.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.MultiTenancy.Persistence;

public abstract class FgsTenantFilteredDbContext : DbContext
{
    private readonly ITenantContextAccessor _tenantContextAccessor;

    protected FgsTenantFilteredDbContext(
        DbContextOptions options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options)
    {
        _tenantContextAccessor = tenantContextAccessor;
    }

    /// <summary>
    /// Evaluated per query on the active DbContext instance (required for EF global filters).
    /// </summary>
    private bool FgsTenantCompanyFilterEnabled =>
        _tenantContextAccessor.Current is { IsResolved: true };

    private long FgsFilterTenantId =>
        _tenantContextAccessor.Current is { IsResolved: true } ctx ? ctx.TenantId : 0;

    private long FgsFilterCompanyId =>
        _tenantContextAccessor.Current is { IsResolved: true } ctx ? ctx.CompanyId : 0;

    protected void ApplyFgsTenantQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null)
            {
                continue;
            }

            if (typeof(ITenantCompanyScoped).IsAssignableFrom(clrType))
            {
                InvokeConfigure(
                    nameof(ConfigureTenantCompanyFilterInternal),
                    clrType,
                    modelBuilder);
            }
            else if (typeof(ITenantScoped).IsAssignableFrom(clrType))
            {
                InvokeConfigure(
                    nameof(ConfigureTenantFilterInternal),
                    clrType,
                    modelBuilder);
            }
        }
    }

    protected void ApplyFgsNullableTenantCompanyQueryFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, INullableTenantCompanyScoped
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            !FgsTenantCompanyFilterEnabled
            || (entity.TenantId == null && entity.CompanyId == null)
            || (entity.TenantId == FgsFilterTenantId && entity.CompanyId == FgsFilterCompanyId));
    }

    private void InvokeConfigure(string methodName, Type clrType, ModelBuilder modelBuilder)
    {
        var method = typeof(FgsTenantFilteredDbContext).GetMethod(
            methodName,
            BindingFlags.NonPublic | BindingFlags.Instance)!.MakeGenericMethod(clrType);

        method.Invoke(this, [modelBuilder]);
    }

    private void ConfigureTenantCompanyFilterInternal<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenantCompanyScoped
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            !FgsTenantCompanyFilterEnabled
            || (entity.TenantId == FgsFilterTenantId && entity.CompanyId == FgsFilterCompanyId));
    }

    private void ConfigureTenantFilterInternal<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenantScoped
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            !FgsTenantCompanyFilterEnabled || entity.TenantId == FgsFilterTenantId);
    }
}
