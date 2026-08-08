using Fgs.Kernel.Entities;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Fgs.MultiTenancy.Persistence;

public abstract class FgsTenantFilteredDbContext : DbContext
{
    private readonly ITenantContextAccessor _tenantContextAccessor;
    private readonly ISoftDeleteFilterAccessor _softDeleteFilterAccessor;

    protected FgsTenantFilteredDbContext(
        DbContextOptions options,
        ITenantContextAccessor tenantContextAccessor)
        : this(options, tenantContextAccessor, new SoftDeleteFilterAccessor())
    {
    }

    protected FgsTenantFilteredDbContext(
        DbContextOptions options,
        ITenantContextAccessor tenantContextAccessor,
        ISoftDeleteFilterAccessor softDeleteFilterAccessor)
        : base(options)
    {
        _tenantContextAccessor = tenantContextAccessor;
        _softDeleteFilterAccessor = softDeleteFilterAccessor;
    }

    /// <summary>
    /// Evaluated per query on the active DbContext instance (required for EF global filters).
    /// </summary>
    private bool FgsTenantCompanyFilterEnabled =>
        _tenantContextAccessor.Current is not null;

    private long FgsFilterTenantId =>
        _tenantContextAccessor.Current?.TenantId ?? 0;

    private long FgsFilterCompanyId =>
        _tenantContextAccessor.Current?.CompanyId ?? 0;

    private bool FgsSoftDeleteFilterEnabled =>
        _softDeleteFilterAccessor.IsEnabled;

    protected void ApplyFgsTenantQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null)
            {
                continue;
            }

            var isSoftDeletable = typeof(ISoftDeletable).IsAssignableFrom(clrType)
                && entityType.FindProperty(nameof(ISoftDeletable.IsActive)) is not null;

            if (typeof(ITenantCompanyScoped).IsAssignableFrom(clrType))
            {
                InvokeConfigure(
                    isSoftDeletable
                        ? nameof(ConfigureTenantCompanySoftDeleteFilterInternal)
                        : nameof(ConfigureTenantCompanyFilterInternal),
                    clrType,
                    modelBuilder);
            }
            else if (typeof(ITenantScoped).IsAssignableFrom(clrType))
            {
                InvokeConfigure(
                    isSoftDeletable
                        ? nameof(ConfigureTenantSoftDeleteFilterInternal)
                        : nameof(ConfigureTenantFilterInternal),
                    clrType,
                    modelBuilder);
            }
            else if (isSoftDeletable)
            {
                InvokeConfigure(nameof(ConfigureSoftDeleteFilterInternal), clrType, modelBuilder);
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

    private void ConfigureTenantCompanySoftDeleteFilterInternal<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenantCompanyScoped, ISoftDeletable
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            (!FgsTenantCompanyFilterEnabled
                || (entity.TenantId == FgsFilterTenantId && entity.CompanyId == FgsFilterCompanyId))
            && (!FgsSoftDeleteFilterEnabled || entity.IsActive));
    }

    private void ConfigureTenantFilterInternal<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenantScoped
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            !FgsTenantCompanyFilterEnabled || entity.TenantId == FgsFilterTenantId);
    }

    private void ConfigureTenantSoftDeleteFilterInternal<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenantScoped, ISoftDeletable
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            (!FgsTenantCompanyFilterEnabled || entity.TenantId == FgsFilterTenantId)
            && (!FgsSoftDeleteFilterEnabled || entity.IsActive));
    }

    private void ConfigureSoftDeleteFilterInternal<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ISoftDeletable
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity =>
            !FgsSoftDeleteFilterEnabled || entity.IsActive);
    }
}
