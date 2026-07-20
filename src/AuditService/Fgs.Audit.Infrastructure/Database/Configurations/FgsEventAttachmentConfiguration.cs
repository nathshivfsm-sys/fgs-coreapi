using Fgs.Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Audit.Infrastructure.Database.Configurations;

internal sealed class FgsEventAttachmentConfiguration : IEntityTypeConfiguration<FgsEventAttachment>
{
    public void Configure(EntityTypeBuilder<FgsEventAttachment> entity)
    {
        entity.ToTable(
            "FgsEventAttachment",
            t => t.HasComment(
                "Associates documents with audit events. Document metadata and storage are managed by the Document Service."));

        entity.HasKey(e => e.Id).HasName("PK_FgsEventAttachment");
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 1, incrementBy: 1)
            .HasComment("Unique identifier of the event attachment.");

        entity.Property(e => e.EventId).IsRequired()
            .HasComment("References the audit event associated with the document.");
        entity.Property(e => e.DocumentId).IsRequired()
            .HasComment("References the associated document managed by the Document Service.");
        entity.Property(e => e.Description).HasMaxLength(500)
            .HasComment("Optional description explaining why the document is associated with the event.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired()
            .HasComment("Date and time the attachment association was created.");

        entity.HasIndex(e => e.EventId)
            .HasDatabaseName("IX_FgsEventAttachment_EventId");
        entity.HasIndex(e => e.DocumentId)
            .HasDatabaseName("IX_FgsEventAttachment_DocumentId");
    }
}
