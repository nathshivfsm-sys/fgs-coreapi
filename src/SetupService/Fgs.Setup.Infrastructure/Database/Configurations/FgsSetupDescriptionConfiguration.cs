using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupDescriptionConfiguration : IEntityTypeConfiguration<FgsSetupDescription>
{
    public void Configure(EntityTypeBuilder<FgsSetupDescription> entity)
    {
        entity.ToTable("FgsSetupDescription");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.Property(e => e.ShortNote).HasMaxLength(30);
        entity.HasOne<FgsSetupTechTrade>()
            .WithMany()
            .HasForeignKey(e => e.FgsSetupTechTradeId)
            .HasConstraintName("FK_FgsSetupDescription_TechTrade")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DescriptionTypeCode })
            .HasDatabaseName("IX_FgsSetupDescription_Tenant_Company_Type");
        entity.HasIndex(e => e.FgsSetupTechTradeId)
            .HasDatabaseName("IX_FgsSetupDescription_TechTrade");
    }
}
