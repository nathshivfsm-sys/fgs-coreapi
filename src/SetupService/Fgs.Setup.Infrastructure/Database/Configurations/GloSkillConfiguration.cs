using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSkillConfiguration : IEntityTypeConfiguration<GloSkill>
{
    public void Configure(EntityTypeBuilder<GloSkill> entity)
    {
        entity.ToTable("GloSkill");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.SkillCode)
            .IsUnique()
            .HasDatabaseName("UX_GloSkill_SkillCode");
        entity.Property(e => e.SkillCode).HasMaxLength(50);
        entity.Property(e => e.SkillName).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.RequiresCertification).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloSkill_GloBusinessType_BusinessTypeId")
            .OnDelete(DeleteBehavior.NoAction);
        entity.HasOne<GloTrade>()
            .WithMany()
            .HasForeignKey(e => e.TradeId)
            .HasConstraintName("FK_GloSkill_GloTrade_TradeId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
