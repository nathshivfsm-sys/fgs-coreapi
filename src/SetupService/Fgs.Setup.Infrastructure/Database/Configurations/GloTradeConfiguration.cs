using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloTradeConfiguration : IEntityTypeConfiguration<GloTrade>
{
    public void Configure(EntityTypeBuilder<GloTrade> entity)
    {
        entity.ToTable("GloTrade");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.TradeCode)
            .IsUnique()
            .HasDatabaseName("UX_GloTrade_TradeCode");
        entity.Property(e => e.TradeCode).HasMaxLength(50);
        entity.Property(e => e.TradeName).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloTrade_GloBusinessType_BusinessTypeId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
