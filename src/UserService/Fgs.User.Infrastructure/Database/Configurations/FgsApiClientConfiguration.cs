using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsApiClientConfiguration : IEntityTypeConfiguration<FgsApiClient>
{
    public void Configure(EntityTypeBuilder<FgsApiClient> entity)
    {
        entity.ToTable(
            "FgsApiClient",
            t => t.HasComment(
                "Stores developer applications created by tenant administrators for third-party integrations. Represents an application, not a credential."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.ClientId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasComment("Public client identifier used by external applications during authentication.");
        entity.Property(e => e.ApplicationName)
            .HasMaxLength(100)
            .HasComment("Display name of the application registered by the customer.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the application.");
        entity.Property(e => e.ContactName)
            .HasMaxLength(100)
            .HasComment("Primary contact responsible for the application.");
        entity.Property(e => e.ContactEmail)
            .HasMaxLength(300)
            .HasComment("Email address of the application owner or support contact.");
        entity.Property(e => e.RateLimitPerMinute)
            .HasDefaultValue(60)
            .HasComment("Maximum number of API requests permitted per minute for this application.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the application is permitted to authenticate and access the API.");

        entity.HasIndex(e => e.ClientId)
            .IsUnique()
            .HasDatabaseName("UX_FgsApiClient_ClientId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ApplicationName })
            .IsUnique()
            .HasDatabaseName("UX_FgsApiClient_TenantId_CompanyId_ApplicationName");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsApiClient_TenantId_CompanyId");
    }
}
