using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupCommunicationTemplateConfiguration : IEntityTypeConfiguration<FgsSetupCommunicationTemplate>
{
    public void Configure(EntityTypeBuilder<FgsSetupCommunicationTemplate> entity)
    {
        entity.ToTable(
            "FgsSetupCommunicationTemplate",
            t => t.HasCheckConstraint(
                "CK_FgsSetupCommunicationTemplate_CommunicationChannel",
                "\"CommunicationChannel\" IN ('Email', 'SMS', 'PushNotification', 'SystemNotification')"));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.CommunicationChannel).HasMaxLength(25);
        entity.Property(e => e.TemplateType).HasColumnType("text");
        entity.Property(e => e.Code).HasColumnType("text");
        entity.Property(e => e.Name).HasColumnType("text");
        entity.Property(e => e.Subject).HasColumnType("text");
        entity.Property(e => e.Body).HasColumnType("text");
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSetupCommunicationTemplate_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CommunicationChannel, e.TemplateType, e.Code })
            .IsUnique()
            .HasDatabaseName("IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTyp");
    }
}
