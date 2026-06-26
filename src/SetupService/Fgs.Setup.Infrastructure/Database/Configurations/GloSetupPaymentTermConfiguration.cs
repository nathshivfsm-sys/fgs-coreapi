using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSetupPaymentTermConfiguration : IEntityTypeConfiguration<GloSetupPaymentTerm>
{
    public void Configure(EntityTypeBuilder<GloSetupPaymentTerm> entity)
    {
        entity.ToTable("GloSetupPaymentTerm");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityAlwaysColumn();
        entity.HasAlternateKey(e => e.Name).HasName("UQ_GloSetupPaymentTerm_Name");
        entity.Property(e => e.Name).HasColumnType("text");
        entity.Property(e => e.DueDateMethod).HasColumnType("text");
        entity.Property(e => e.IsAccountsReceivable).HasDefaultValue(true);
        entity.Property(e => e.IsAccountsPayable).HasDefaultValue(false);
        entity.Property(e => e.IsMobileVisible).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("timezone('utc', now())");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
