using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Notification.Infrastructure.Database.Migrations;

[DbContext(typeof(FgsNotificationDbContext))]
[Migration("20260516180000_InitialPlatform")]
partial class InitialPlatform
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasDefaultSchema("dbo")
            .HasAnnotation("ProductVersion", "10.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("Fgs.Notification.Domain.Entities.FgsNotificationHistory", b =>
        {
            b.Property<Guid>("Id").HasColumnType("uuid");
            b.Property<int>("Channel").HasColumnType("integer");
            b.Property<string>("CorrelationId").HasMaxLength(64).HasColumnType("character varying(64)");
            b.Property<DateTimeOffset>("CreatedOn").HasColumnType("timestamp with time zone");
            b.Property<string>("Error").HasMaxLength(2000).HasColumnType("character varying(2000)");
            b.Property<string>("ProviderMessageId").HasMaxLength(256).HasColumnType("character varying(256)");
            b.Property<string>("Recipient").HasMaxLength(512).HasColumnType("character varying(512)");
            b.Property<DateTimeOffset?>("SentOn").HasColumnType("timestamp with time zone");
            b.Property<int>("Status").HasColumnType("integer");
            b.Property<string>("TemplateName").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)");
            b.Property<Guid>("TenantId").HasColumnType("uuid");
            b.HasKey("Id");
            b.HasIndex("TenantId", "CreatedOn");
            b.ToTable("FgsNotificationHistory", "dbo");
        });

        modelBuilder.Entity("Fgs.Notification.Domain.Entities.FgsProcessedIntegrationEvent", b =>
        {
            b.Property<Guid>("Id").HasColumnType("uuid");
            b.Property<string>("EventType").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)");
            b.Property<string>("MessageId").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)");
            b.Property<DateTimeOffset>("ProcessedOn").HasColumnType("timestamp with time zone");
            b.HasKey("Id");
            b.HasIndex("MessageId").IsUnique();
            b.ToTable("FgsProcessedIntegrationEvent", "dbo");
        });
#pragma warning restore 612, 618
    }
}
