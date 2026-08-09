using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Database.Configurations;
using Fgs.Billing.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Billing.Infrastructure.Database;

public sealed class FgsBillingDbContext : FgsTenantFilteredDbContext
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public FgsBillingDbContext(
        DbContextOptions<FgsBillingDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<FgsInvoice> FgsInvoices => Set<FgsInvoice>();

    public DbSet<FgsInvoiceDetail> FgsInvoiceDetails => Set<FgsInvoiceDetail>();

    public DbSet<FgsInvoiceWorkDescription> FgsInvoiceWorkDescriptions => Set<FgsInvoiceWorkDescription>();

    public DbSet<FgsInvoiceBatch> FgsInvoiceBatches => Set<FgsInvoiceBatch>();

    public DbSet<FgsPayment> FgsPayments => Set<FgsPayment>();

    public DbSet<FgsInvoicePaymentApplication> FgsInvoicePaymentApplications => Set<FgsInvoicePaymentApplication>();

    public DbSet<FgsPaymentTransaction> FgsPaymentTransactions => Set<FgsPaymentTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Billing);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsBillingDbContext).Assembly);
        FgsBillingDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
