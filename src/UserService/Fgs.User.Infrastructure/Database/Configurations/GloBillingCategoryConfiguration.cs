using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class GloBillingCategoryConfiguration : IEntityTypeConfiguration<GloBillingCategory>
{
    public void Configure(EntityTypeBuilder<GloBillingCategory> entity)
    {
        entity.ToTable("GloBillingCategory");
        entity.HasKey(e => e.BillingCategoryType);
        entity.Property(e => e.BillingCategoryType).HasMaxLength(2);
        entity.Property(e => e.BillingCategoryName).HasMaxLength(100);
    }
}
