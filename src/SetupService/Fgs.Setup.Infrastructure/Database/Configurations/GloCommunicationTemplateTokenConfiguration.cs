using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloCommunicationTemplateTokenConfiguration : IEntityTypeConfiguration<GloCommunicationTemplateToken>
{
    public void Configure(EntityTypeBuilder<GloCommunicationTemplateToken> entity)
    {
        entity.ToTable(
            "GloCommunicationTemplateToken",
            t => t.HasComment(
                "Junction table defining the valid communication tokens that may be used within a communication template."));

        entity.HasKey(e => new { e.CommunicationTemplateId, e.CommunicationTokenId })
            .HasName("PK_GloCommunicationTemplateToken");

        entity.Property(e => e.CommunicationTemplateId)
            .HasComment("Reference to the communication template.");
        entity.Property(e => e.CommunicationTokenId)
            .HasComment("Reference to a communication token available for use within the template.");

        entity.HasOne(e => e.CommunicationTemplate)
            .WithMany(t => t.TemplateTokens)
            .HasForeignKey(e => e.CommunicationTemplateId)
            .HasConstraintName("FK_GloCommunicationTemplateToken_CommunicationTemplateId")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.CommunicationToken)
            .WithMany()
            .HasForeignKey(e => e.CommunicationTokenId)
            .HasConstraintName("FK_GloCommunicationTemplateToken_CommunicationTokenId")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => e.CommunicationTokenId)
            .HasDatabaseName("IX_GloCommunicationTemplateToken_CommunicationTokenId");
    }
}
