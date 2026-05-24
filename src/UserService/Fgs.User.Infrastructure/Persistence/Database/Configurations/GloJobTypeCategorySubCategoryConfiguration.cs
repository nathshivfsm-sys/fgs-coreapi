using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloJobTypeCategorySubCategoryConfiguration : IEntityTypeConfiguration<GloJobTypeCategorySubCategory>
{
    public void Configure(EntityTypeBuilder<GloJobTypeCategorySubCategory> entity)
    {
        entity.ToTable("GloJobTypeCategorySubCategory");
        entity.HasKey(e => new { e.BusinessTypeId, e.CategoryId, e.SubCategoryId });
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<GloJobTypeCategory>()
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName("FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<GloJobTypeSubCategory>()
            .WithMany()
            .HasForeignKey(e => e.SubCategoryId)
            .HasConstraintName("FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => e.CategoryId)
            .HasDatabaseName("IX_GloJobTypeCategorySubCategory_CategoryId");
        entity.HasIndex(e => e.SubCategoryId)
            .HasDatabaseName("IX_GloJobTypeCategorySubCategory_SubCategoryId");
    }
}
