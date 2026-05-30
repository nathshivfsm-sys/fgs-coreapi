using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloBillingCategoryConfiguration : IEntityTypeConfiguration<GloBillingCategory>
{
    public void Configure(EntityTypeBuilder<GloBillingCategory> entity)
    {
        entity.ToTable("GloBillingCategory", t =>
            t.HasComment("Global billing line category lookup used during tenant provisioning (equipment, labor, tax, etc.)."));

        entity.HasKey(e => e.BillingCategoryType);

        entity.Property(e => e.BillingCategoryType)
            .HasMaxLength(2)
            .HasComment("Short billing category code (primary key), e.g. IN, LB, TX.");

        entity.Property(e => e.BillingCategoryName)
            .HasMaxLength(100)
            .HasComment("Display name of the billing category.");

        entity.Property(e => e.Description)
            .HasColumnType("text")
            .HasComment("Optional description of how the billing category is used.");

        entity.Property(e => e.DisplayOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)1)
            .HasComment("Controls sorting/display order of billing categories in dropdowns and setup screens.");

        entity.Property(e => e.ShowToFieldTech)
            .HasDefaultValue(true)
            .HasComment(
                "Determines whether field technicians can view/select this billing category in mobile and field workflows.");

        entity.Property(e => e.AllowToPick)
            .HasDefaultValue(true)
            .HasComment(
                "Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.");
    }
}
