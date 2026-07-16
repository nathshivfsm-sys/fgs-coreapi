using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsPublicEndpointConfiguration : IEntityTypeConfiguration<FgsPublicEndpoint>
{
    public void Configure(EntityTypeBuilder<FgsPublicEndpoint> entity)
    {
        entity.ToTable(
            "FgsPublicEndpoint",
            t => t.HasComment(
                "Stores public endpoints exposed by the platform for each tenant and company. Used during authentication and by client applications to discover the appropriate application or integration endpoint."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the service endpoint.");
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Tenant that owns the service endpoint.");
        entity.Property(e => e.CompanyId)
            .HasComment("Company that owns the service endpoint.");
        entity.Property(e => e.EndpointType)
            .HasMaxLength(50)
            .HasComment("Type of public endpoint. Supported values are BFF for the application backend and API for third-party integrations.");
        entity.Property(e => e.EnvironmentCode)
            .HasMaxLength(20)
            .HasComment("Deployment environment of the endpoint. Supported values are PROD, SANDBOX, TRAINING, QA, PREVIEW and DEVELOPMENT.");
        entity.Property(e => e.BaseUrl)
            .HasMaxLength(500)
            .HasComment("Base URL clients use to access the public endpoint.");
        entity.Property(e => e.DisplayName)
            .HasMaxLength(100)
            .HasComment("User-friendly name displayed when multiple environments are available.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the endpoint is available for use.");
        entity.Property(e => e.CreatedOn)
            .HasComment("Date and time the service endpoint was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or system that created the service endpoint.");
        entity.Property(e => e.UpdatedOn)
            .HasComment("Date and time the service endpoint was last modified.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or system that last modified the service endpoint.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EndpointType, e.EnvironmentCode })
            .IsUnique()
            .HasDatabaseName("IX_FgsPublicEndpoint_Tenant_Company_Type_Environment");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPublicEndpoint_Tenant_Company");
    }
}
