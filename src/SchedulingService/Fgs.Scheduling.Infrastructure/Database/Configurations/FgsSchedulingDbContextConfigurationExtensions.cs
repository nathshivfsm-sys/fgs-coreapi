using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal static class FgsSchedulingDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped =>
        EntityFrameworkExtensions.ConfigureTenantCompanyColumns(entity);

    internal static void ConfigureTenantCompanyCacheFkNonGeneric(
        this EntityTypeBuilder entity,
        string constraintName) =>
        entity.ConfigureTenantCompanyCacheFkNonGeneric(typeof(FgsTenantCompanyCache), constraintName);

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type> { typeof(FgsTenantCompanyCache) },
            tableName => tableName switch
            {
                "FgsWorkOrder" => "FK_FgsWorkOrder_TenantCompany",
                "FgsWorkOrderAsset" => "FK_FgsWorkOrderAsset_TenantCompany",
                "FgsAppointment" => "FK_FgsAppointment_TenantCompany",
                "FgsAppointmentAssignment" => "FK_FgsAppointmentAssignment_TenantCompany",
                "FgsAppointmentAssignmentEvent" => "FK_FgsAppointmentAssignmentEvent_FgsTenantCompanyCache",
                "FgsPayrollPayPeriod" => "FK_FgsPayrollPayPeriod_FgsTenantCompanyCache",
                "FgsPayroll" => "FK_FgsPayroll_FgsTenantCompanyCache",
                "FgsPayrollLine" => "FK_FgsPayrollLine_FgsTenantCompanyCache",
                _ => $"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId"
            });

    internal static void ConfigureAuditActorColumns(ModelBuilder modelBuilder, int maxLength = 100) =>
        modelBuilder.ConfigureAuditActorColumns(maxLength);
}
