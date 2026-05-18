using Fgs.Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Platform.Infrastructure.Database.Migrations;

[DbContext(typeof(FgsPlatformDbContext))]
partial class FgsPlatformDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasDefaultSchema("dbo")
            .HasAnnotation("ProductVersion", "10.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("Fgs.Platform.Domain.Entities.FgsNotificationHistory", b =>
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

        modelBuilder.Entity("Fgs.Platform.Domain.Entities.FgsProcessedIntegrationEvent", b =>
        {
            b.Property<Guid>("Id").HasColumnType("uuid");
            b.Property<string>("EventType").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)");
            b.Property<string>("MessageId").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)");
            b.Property<DateTimeOffset>("ProcessedOn").HasColumnType("timestamp with time zone");
            b.HasKey("Id");
            b.HasIndex("MessageId").IsUnique();
            b.ToTable("FgsProcessedIntegrationEvent", "dbo");
        });

        modelBuilder.Entity("Fgs.Platform.Domain.Entities.FgsSetupCommunicationTemplate", b =>
        {
            b.Property<long>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("bigint");

            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

            b.Property<string>("Body").IsRequired().HasColumnType("text");
            b.Property<string>("Code").IsRequired().HasColumnType("text");
            b.Property<Guid?>("CompanyId").HasColumnType("uuid");
            b.Property<Guid?>("CreatedBy").HasColumnType("uuid");
            b.Property<DateTimeOffset>("CreatedOn").HasColumnType("timestamp with time zone");
            b.Property<bool>("IsActive").HasColumnType("boolean");
            b.Property<bool>("IsMobileVisible").HasColumnType("boolean");
            b.Property<string>("Name").IsRequired().HasColumnType("text");
            b.Property<string>("Subject").HasColumnType("text");
            b.Property<string>("TemplateType").IsRequired().HasColumnType("text");
            b.Property<Guid?>("TenantId").HasColumnType("uuid");
            b.Property<Guid?>("UpdatedBy").HasColumnType("uuid");
            b.Property<DateTimeOffset?>("UpdatedOn").HasColumnType("timestamp with time zone");
            b.HasKey("Id");
            b.HasIndex("TenantId", "CompanyId");
            b.HasIndex("TenantId", "CompanyId", "TemplateType", "Code")
                .IsUnique();
            b.ToTable("FgsSetupCommunicationTemplate", "dbo");
        });
#pragma warning restore 612, 618
    }
}
