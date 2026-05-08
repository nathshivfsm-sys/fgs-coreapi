using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainCompanyConfiguration = UserService.Domain.Entities.CompanyConfiguration;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class CompanyConfigurationEntityConfiguration : IEntityTypeConfiguration<DomainCompanyConfiguration>
{
    public void Configure(EntityTypeBuilder<DomainCompanyConfiguration> builder)
    {
        builder.ToTable("CompanyConfiguration");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.CompanyId).IsRequired();
        builder.Property(e => e.TimeCardOptionId).IsRequired();
        builder.Property(e => e.AccountingIntegrationTypeId);
        builder.Property(e => e.EnableCallBookingWidget).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.EnablePaymentWidget).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.EnableCustomerPortal).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.EnableRulesManagement).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.EnableAutoArrive).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.WorkLocationRadiusForAutoArrive);
        builder.Property(e => e.OTStartTime);
        builder.Property(e => e.OTEndTime);
        builder.Property(e => e.DTStartTime);
        builder.Property(e => e.DTEndTime);
        builder.Property(e => e.BillHoursFromDispatchOrArrive).HasMaxLength(20).IsRequired().HasDefaultValue("DISPATCH");
        builder.Property(e => e.SourceCodeRequiredOnWorkOrder).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.SourceCodeRequiredOnServiceLocation).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.BillToStartNumber).IsRequired().HasDefaultValue(100L);
        builder.Property(e => e.POStartNumber).IsRequired().HasDefaultValue(100L);
        builder.Property(e => e.QuoteStartNumber).IsRequired().HasDefaultValue(100L);
        builder.Property(e => e.WorkOrderStartNumber).IsRequired().HasDefaultValue(100L);
        builder.Property(e => e.InvoiceNumberPrefix).HasMaxLength(20);
        builder.Property(e => e.QuoteNumberPrefix).HasMaxLength(20);
        builder.Property(e => e.PONumberPrefix).HasMaxLength(20);
        builder.Property(e => e.WorkOrderNumberPrefix).HasMaxLength(20);
        builder.Property(e => e.InvoiceBatchNumberFormat).HasMaxLength(200);
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.CreatedOn).IsRequired();
        builder.Property(e => e.UpdatedOn);

        builder.HasIndex(e => e.CompanyId).IsUnique().HasDatabaseName("IxCompanyConfigurationCompany");
        builder.HasIndex(e => e.AccountingIntegrationTypeId)
            .HasDatabaseName("IxCompanyConfigurationAccountingIntegrationTypeId");
        builder.HasIndex(e => e.TimeCardOptionId).HasDatabaseName("IxCompanyConfigurationTimeCardOptionId");

        builder.HasOne(e => e.Company)
            .WithOne(e => e.Configuration)
            .HasForeignKey<DomainCompanyConfiguration>(e => e.CompanyId)
            .HasConstraintName("FK_CompanyConfiguration_Company")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TimeCardOption)
            .WithMany()
            .HasForeignKey(e => e.TimeCardOptionId)
            .HasConstraintName("FK_CompanyConfiguration_TimeCardOption")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.AccountingIntegrationType)
            .WithMany()
            .HasForeignKey(e => e.AccountingIntegrationTypeId)
            .HasConstraintName("FK_CompanyConfiguration_AccountingIntegrationType")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
