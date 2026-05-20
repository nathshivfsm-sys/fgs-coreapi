using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloPaymentMethodTypeConfiguration : IEntityTypeConfiguration<GloPaymentMethodType>
{
    public void Configure(EntityTypeBuilder<GloPaymentMethodType> entity)
    {
        entity.ToTable("GloPaymentMethodType");
        entity.HasKey(e => e.Id);
        entity.HasAlternateKey(e => e.Code).HasName("UQ_GloPaymentMethodType_Code");
    }
}
