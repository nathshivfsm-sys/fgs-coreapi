using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsApiWebhookSubscriptionConfiguration : IEntityTypeConfiguration<FgsApiWebhookSubscription>
{
    public void Configure(EntityTypeBuilder<FgsApiWebhookSubscription> entity)
    {
        entity.ToTable(
            "FgsApiWebhookSubscription",
            t => t.HasComment("Assigns one or more API events to webhook endpoints for event delivery."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the webhook subscription was created.");
        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("User or system that created the webhook subscription.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsApiWebhookId, e.FgsApiEventId })
            .IsUnique()
            .HasDatabaseName("IX_FgsApiWebhookSubscription_TenantId_CompanyId_Webhook_Event");
        entity.HasIndex(e => e.FgsApiWebhookId)
            .HasDatabaseName("IX_FgsApiWebhookSubscription_FgsApiWebhookId");
        entity.HasIndex(e => e.FgsApiEventId)
            .HasDatabaseName("IX_FgsApiWebhookSubscription_FgsApiEventId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsApiWebhookSubscription_TenantId_CompanyId");

        entity.HasOne(e => e.FgsApiWebhook)
            .WithMany(w => w.Subscriptions)
            .HasForeignKey(e => e.FgsApiWebhookId)
            .HasConstraintName("FK_FgsApiWebhookSubscription_FgsApiWebhook")
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.FgsApiEvent)
            .WithMany()
            .HasForeignKey(e => e.FgsApiEventId)
            .HasConstraintName("FK_FgsApiWebhookSubscription_FgsApiEvent")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
