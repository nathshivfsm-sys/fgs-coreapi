using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsApiRequestLogConfiguration : IEntityTypeConfiguration<FgsApiRequestLog>
{
    public void Configure(EntityTypeBuilder<FgsApiRequestLog> entity)
    {
        entity.ToTable(
            "FgsApiRequestLog",
            t => t.HasComment(
                "Stores API request metadata for monitoring, troubleshooting, rate limiting and analytics."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.RequestId)
            .HasComment("Unique identifier used to correlate request processing across services.");
        entity.Property(e => e.Resource)
            .HasMaxLength(100)
            .HasComment("Business resource targeted by the API request, such as WorkOrder, Customer, Estimate or Invoice. Used for reporting, analytics, monitoring and rate limiting.");
        entity.Property(e => e.HttpMethod)
            .HasMaxLength(10)
            .HasComment("HTTP method used by the request, such as GET, POST, PUT or DELETE.");
        entity.Property(e => e.Endpoint)
            .HasMaxLength(255)
            .HasComment("API endpoint requested by the client.");
        entity.Property(e => e.HttpStatusCode)
            .HasComment("HTTP response status code returned to the client.");
        entity.Property(e => e.DurationMilliseconds)
            .HasComment("Total request processing time in milliseconds.");
        entity.Property(e => e.ClientIpAddress)
            .HasMaxLength(50)
            .HasComment("IP address from which the API request originated.");
        entity.Property(e => e.UserAgent)
            .HasMaxLength(500)
            .HasComment("User-Agent header supplied by the client application.");
        entity.Property(e => e.ErrorCode)
            .HasMaxLength(100)
            .HasComment("Application-specific error code returned for failed requests.");
        entity.Property(e => e.ErrorMessage)
            .HasMaxLength(500)
            .HasComment("Brief error message associated with the failed request.");
        entity.Property(e => e.RequestedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the API request was received.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsApiRequestLog_TenantId_CompanyId");
        entity.HasIndex(e => e.FgsApiClientId)
            .HasDatabaseName("IX_FgsApiRequestLog_FgsApiClientId");
        entity.HasIndex(e => e.RequestId)
            .IsUnique()
            .HasDatabaseName("IX_FgsApiRequestLog_RequestId");
        entity.HasIndex(e => e.RequestedOn)
            .HasDatabaseName("IX_FgsApiRequestLog_RequestedOn");
        entity.HasIndex(e => e.HttpStatusCode)
            .HasDatabaseName("IX_FgsApiRequestLog_HttpStatusCode");

        entity.HasOne(e => e.FgsApiClient)
            .WithMany(c => c.RequestLogs)
            .HasForeignKey(e => e.FgsApiClientId)
            .HasConstraintName("FK_FgsApiRequestLog_FgsApiClient")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
