using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloCategorySubCategoryConfiguration : IEntityTypeConfiguration<GloCategorySubCategory>
{
    public void Configure(EntityTypeBuilder<GloCategorySubCategory> entity)
    {
        entity.ToTable("GloCategorySubCategory");
        entity.HasKey(e => new { e.BusinessTypeId, e.CategoryId, e.SubCategoryId });
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloCategorySubCategory_GloBusinessType_BusinessTypeId")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<GloCategory>()
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName("FK_GloCategorySubCategory_GloCategory_CategoryId")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<GloSubCategory>()
            .WithMany()
            .HasForeignKey(e => e.SubCategoryId)
            .HasConstraintName("FK_GloCategorySubCategory_GloSubCategory_SubCategoryId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
