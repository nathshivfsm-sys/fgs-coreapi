using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsApiWebhookConfiguration : IEntityTypeConfiguration<FgsApiWebhook>
{
    public void Configure(EntityTypeBuilder<FgsApiWebhook> entity)
    {
        entity.ToTable(
            "FgsApiWebhook",
            t => t.HasComment(
                "Stores webhook endpoints registered by tenant administrators for receiving API event notifications."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("Display name of the webhook endpoint.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the webhook endpoint.");
        entity.Property(e => e.EndpointUrl)
            .HasMaxLength(500)
            .HasComment("HTTPS endpoint that receives webhook event notifications.");
        entity.Property(e => e.AuthenticationType)
            .HasMaxLength(30)
            .HasComment("Authentication method used when invoking the webhook endpoint, such as None, BearerToken, BasicAuthentication or CustomHeader.");
        entity.Property(e => e.AuthenticationValue)
            .HasMaxLength(500)
            .HasComment("Authentication value associated with the selected authentication type.");
        entity.Property(e => e.Secret)
            .HasMaxLength(255)
            .HasComment("Shared secret used to sign webhook requests and verify message authenticity.");
        entity.Property(e => e.TimeoutSeconds)
            .HasDefaultValue((short)30)
            .HasComment("Maximum number of seconds to wait for the webhook endpoint to respond before the request is considered failed.");
        entity.Property(e => e.MaximumRetryCount)
            .HasDefaultValue((short)5)
            .HasComment("Maximum number of retry attempts after a webhook delivery failure.");
        entity.Property(e => e.LastSuccessfulDeliveryOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the most recent webhook event was successfully delivered to this endpoint.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the webhook endpoint is enabled and eligible to receive events.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsApiWebhook_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsApiWebhook_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsApiWebhook_IsActive");
    }
}
