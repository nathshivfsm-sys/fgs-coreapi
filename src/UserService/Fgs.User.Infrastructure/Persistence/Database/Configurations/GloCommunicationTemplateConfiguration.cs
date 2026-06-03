using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloCommunicationTemplateConfiguration : IEntityTypeConfiguration<GloCommunicationTemplate>
{
    public void Configure(EntityTypeBuilder<GloCommunicationTemplate> entity)
    {
        entity.ToTable(
            "GloCommunicationTemplate",
            t =>
            {
                t.HasComment(
                    "Stores FSM-provided communication templates available for system use or tenant customization.");
                t.HasCheckConstraint(
                    "CK_GloCommunicationTemplate_TemplateScope",
                    "\"TemplateScope\" IN ('Tenant', 'System')");
                t.HasCheckConstraint(
                    "CK_GloCommunicationTemplate_CommunicationChannel",
                    "\"CommunicationChannel\" IN ('Email', 'SMS', 'PushNotification', 'SystemNotification')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.TemplateScope)
            .HasMaxLength(20)
            .HasDefaultValue("Tenant")
            .HasComment("Defines whether the template is system-managed or available for tenant customization.");
        entity.Property(e => e.CommunicationChannel)
            .HasMaxLength(25)
            .HasComment(
                "Communication delivery channel such as Email, SMS, PushNotification, or SystemNotification.");
        entity.Property(e => e.TemplateCode)
            .HasMaxLength(100)
            .HasComment(
                "Unique business event identifier such as INVOICE_SENT, PASSWORD_RESET, or WORKORDER_COMPLETED.");
        entity.Property(e => e.Name)
            .HasMaxLength(200)
            .HasComment("Display name of the communication template.");
        entity.Property(e => e.Subject)
            .HasComment("Subject line used for communication channels that support a subject.");
        entity.Property(e => e.Body)
            .HasComment("Template content containing static text and communication tokens.");
        entity.Property(e => e.IsMobileVisible)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the template is available within the mobile application.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Determines the display order of the template in user interfaces.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the communication template is active and available for use.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.CommunicationChannel, e.TemplateCode })
            .IsUnique()
            .HasDatabaseName("UQ_GloCommunicationTemplate_CommunicationChannel_TemplateCode");
        entity.HasIndex(e => e.TemplateScope).HasDatabaseName("IX_GloCommunicationTemplate_TemplateScope");
        entity.HasIndex(e => e.CommunicationChannel)
            .HasDatabaseName("IX_GloCommunicationTemplate_CommunicationChannel");
        entity.HasIndex(e => e.TemplateCode).HasDatabaseName("IX_GloCommunicationTemplate_TemplateCode");
        entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_GloCommunicationTemplate_IsActive");
    }
}
