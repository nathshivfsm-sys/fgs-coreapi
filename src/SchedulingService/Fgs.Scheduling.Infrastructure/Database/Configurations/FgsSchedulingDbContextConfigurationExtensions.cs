using Fgs.Kernel.Entities;
using Fgs.Scheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Scheduling.Infrastructure.Database.Configurations;

internal static class FgsSchedulingDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped
    {
        entity.Property(nameof(ITenantCompanyScoped.TenantId)).HasColumnOrder(1);
        entity.Property(nameof(ITenantCompanyScoped.CompanyId)).HasColumnOrder(2);
    }

    internal static void ConfigureTenantCompanyCacheFkNonGeneric(
        this EntityTypeBuilder entity,
        string constraintName)
    {
        entity.HasOne(typeof(FgsTenantCompanyCache))
            .WithMany()
            .HasForeignKey(nameof(ITenantCompanyScoped.TenantId), nameof(ITenantCompanyScoped.CompanyId))
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder)
    {
        var excludedTypes = new HashSet<Type> { typeof(FgsTenantCompanyCache) };

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null || excludedTypes.Contains(clrType))
            {
                continue;
            }

            if (!typeof(ITenantCompanyScoped).IsAssignableFrom(clrType))
            {
                continue;
            }

            var tableName = entityType.GetTableName();
            if (string.IsNullOrEmpty(tableName))
            {
                continue;
            }

            var constraintName = tableName switch
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
            };

            ((EntityTypeBuilder)modelBuilder.Entity(clrType))
                .ConfigureTenantCompanyCacheFkNonGeneric(constraintName);
        }
    }

    internal static void ConfigureAuditActorColumns(ModelBuilder modelBuilder, int maxLength = 100)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdBy = entityType.FindProperty("CreatedBy");
            if (createdBy?.ClrType == typeof(string))
            {
                createdBy.SetMaxLength(maxLength);
            }

            var updatedBy = entityType.FindProperty("UpdatedBy");
            if (updatedBy?.ClrType == typeof(string))
            {
                updatedBy.SetMaxLength(maxLength);
            }
        }
    }
}
