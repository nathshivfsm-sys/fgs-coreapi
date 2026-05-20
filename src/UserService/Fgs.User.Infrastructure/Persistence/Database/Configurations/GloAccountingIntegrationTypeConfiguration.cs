using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloAccountingIntegrationTypeConfiguration : IEntityTypeConfiguration<GloAccountingIntegrationType>
{
    public void Configure(EntityTypeBuilder<GloAccountingIntegrationType> entity)
    {
        entity.ToTable("GloAccountingIntegrationType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasAlternateKey(e => e.Code)
            .HasName("UX_AccountingIntegrationType_Code");
        entity.Property(e => e.Code).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
