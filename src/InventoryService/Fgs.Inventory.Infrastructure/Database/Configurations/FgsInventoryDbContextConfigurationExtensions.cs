using Fgs.Kernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal static class FgsInventoryDbContextConfigurationExtensions
{
    private static readonly ValueConverter<string?, long?> BigintActorIdConverter = new(
        v => string.IsNullOrWhiteSpace(v) ? null : long.Parse(v),
        v => v.HasValue ? v.Value.ToString() : null);

    internal static void ConfigureGloEntityBigintAuditColumns<T>(this EntityTypeBuilder<T> entity)
        where T : GloEntityBase
    {
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.CreatedBy)
            .HasColumnType("bigint")
            .HasConversion(BigintActorIdConverter);
        entity.Property(e => e.UpdatedBy)
            .HasColumnType("bigint")
            .HasConversion(BigintActorIdConverter);
    }
}
