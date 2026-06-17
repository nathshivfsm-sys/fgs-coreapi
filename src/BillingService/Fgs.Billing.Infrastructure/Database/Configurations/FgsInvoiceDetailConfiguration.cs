using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoiceDetailConfiguration : IEntityTypeConfiguration<FgsInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<FgsInvoiceDetail> entity)
    {
        entity.ToTable("FgsInvoiceDetail");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ItemCode).HasMaxLength(100);
        entity.Property(e => e.ItemDescription).HasColumnType("text").IsRequired();
        entity.Property(e => e.MasterPartNum).HasMaxLength(100);
        entity.Property(e => e.Quantity).HasColumnType("numeric(18,4)").HasDefaultValue(1m);
        entity.Property(e => e.UnitCost).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.ExtendedCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.UnitPrice).HasColumnType("numeric(18,4)").HasDefaultValue(0m);
        entity.Property(e => e.ExtendedPrice).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.IsInventory).HasDefaultValue(false);
        entity.Property(e => e.IsTaxable).HasDefaultValue(false);
        entity.Property(e => e.LineAddedFrom).HasMaxLength(50);
        entity.Property(e => e.AddedSource).HasMaxLength(50);
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasOne<FgsInvoice>()
            .WithMany()
            .HasForeignKey(e => e.InvoiceId)
            .HasConstraintName("FK_FgsInvoiceDetail_Invoice")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsInvoiceDetail>()
            .WithMany()
            .HasForeignKey(e => e.ParentLineId)
            .HasConstraintName("FK_FgsInvoiceDetail_ParentLine")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceId })
            .HasDatabaseName("IX_FgsInvoiceDetail_InvoiceId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceId, e.LineNumber })
            .HasDatabaseName("IX_FgsInvoiceDetail_InvoiceId_LineNumber");
    }
}
