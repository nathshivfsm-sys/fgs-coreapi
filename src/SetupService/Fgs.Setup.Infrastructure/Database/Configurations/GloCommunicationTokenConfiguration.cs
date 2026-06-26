using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloCommunicationTokenConfiguration : IEntityTypeConfiguration<GloCommunicationToken>
{
    public void Configure(EntityTypeBuilder<GloCommunicationToken> entity)
    {
        entity.ToTable("GloCommunicationToken");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.TokenCode).HasColumnType("text");
        entity.Property(e => e.DisplayName).HasColumnType("text");
        entity.Property(e => e.SourceDatabaseName).HasColumnType("text");
        entity.Property(e => e.SourceSchemaName).HasColumnType("text");
        entity.Property(e => e.SourceTableName).HasColumnType("text");
        entity.Property(e => e.SourceColumnName).HasColumnType("text");
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasAlternateKey(e => e.TokenCode).HasName("UQ_GloCommunicationToken_TokenCode");
    }
}
