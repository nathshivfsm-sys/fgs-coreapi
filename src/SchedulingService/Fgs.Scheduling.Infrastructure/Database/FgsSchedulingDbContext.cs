using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Scheduling.Domain.Entities;
using Fgs.Scheduling.Infrastructure.Database.Configurations;
using Fgs.Scheduling.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Scheduling.Infrastructure.Database;

public sealed class FgsSchedulingDbContext : FgsTenantFilteredDbContext
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public FgsSchedulingDbContext(
        DbContextOptions<FgsSchedulingDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();
    public DbSet<FgsWorkOrder> FgsWorkOrders => Set<FgsWorkOrder>();
    public DbSet<FgsWorkOrderAsset> FgsWorkOrderAssets => Set<FgsWorkOrderAsset>();
    public DbSet<FgsWorkOrderItem> FgsWorkOrderItems => Set<FgsWorkOrderItem>();
    public DbSet<FgsWorkOrderIntegration> FgsWorkOrderIntegrations => Set<FgsWorkOrderIntegration>();
    public DbSet<FgsDispatchBoardTechnician> FgsDispatchBoardTechnicians => Set<FgsDispatchBoardTechnician>();
    public DbSet<FgsAppointment> FgsAppointments => Set<FgsAppointment>();
    public DbSet<FgsAppointmentAssignment> FgsAppointmentAssignments => Set<FgsAppointmentAssignment>();
    public DbSet<FgsAppointmentAssignmentEvent> FgsAppointmentAssignmentEvents => Set<FgsAppointmentAssignmentEvent>();
    public DbSet<FgsPayrollPayPeriod> FgsPayrollPayPeriods => Set<FgsPayrollPayPeriod>();
    public DbSet<FgsPayroll> FgsPayrolls => Set<FgsPayroll>();
    public DbSet<FgsPayrollLine> FgsPayrollLines => Set<FgsPayrollLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Dispatch);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsSchedulingDbContext).Assembly);
        FgsSchedulingDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
        FgsSchedulingDbContextConfigurationExtensions.ConfigureAuditActorColumns(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
