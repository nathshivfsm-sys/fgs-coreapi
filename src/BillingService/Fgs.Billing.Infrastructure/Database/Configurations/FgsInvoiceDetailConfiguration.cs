using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoiceDetailConfiguration : IEntityTypeConfiguration<FgsInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<FgsInvoiceDetail> entity)
    {
        entity.ToTable(
            "FgsInvoiceDetail",
            t => t.HasComment(
                "Stores individual invoice line items, including labor, service, equipment, material, and other billable items, along with pricing, cost, tax, accounting, technician, and source information."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the invoice detail line.");

        entity.ConfigureTenantCompanyColumns();
        entity.Property(e => e.TenantId)
            .HasComment("Identifies the tenant that owns the invoice detail.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifies the company within the tenant that owns the invoice detail.");

        entity.Property(e => e.InvoiceId)
            .HasComment("Identifies the invoice to which this detail line belongs.");
        entity.Property(e => e.ParentLineId)
            .HasComment(
                "Identifies the parent invoice detail line when this line is associated with another invoice line, such as a child or related line.");
        entity.Property(e => e.LineNumber)
            .HasComment(
                "Sequential line number used to identify and order the detail lines within an invoice.");
        entity.Property(e => e.BillingCategoryId)
            .HasComment(
                "Identifies the billing category that determines the type and behavior of the invoice line, such as Labor, Service, Equipment, Material, or Other.");
        entity.Property(e => e.ItemCode)
            .HasMaxLength(100)
            .HasComment(
                "Code identifying the service, material, equipment, or other item associated with the invoice line.");
        entity.Property(e => e.ItemDescription)
            .HasColumnType("text")
            .IsRequired()
            .HasComment(
                "Description of the item, service, labor, or charge displayed on the invoice.");
        entity.Property(e => e.IsInventory)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the invoice line represents an inventory item.");
        entity.Property(e => e.MasterPartNum)
            .HasMaxLength(100)
            .HasComment("Master part number associated with the item when applicable.");
        entity.Property(e => e.InventoryItemId)
            .HasComment(
                "Identifies the inventory item associated with the invoice detail when the line represents an inventory item.");
        entity.Property(e => e.PriceBookItemId)
            .HasComment(
                "Identifies the Price Book item from which the invoice line was selected or populated, when applicable.");
        entity.Property(e => e.LaborRateTypeId)
            .HasComment(
                "Identifies the labor rate type used to determine labor pricing when the invoice line is a labor item.");
        entity.Property(e => e.TechnicianId)
            .HasComment(
                "Identifies the technician associated with the invoice line, when applicable.");
        entity.Property(e => e.Quantity)
            .HasColumnType("numeric(18,4)")
            .HasDefaultValue(1m)
            .HasComment(
                "Quantity used to calculate the extended cost and extended sales price of the invoice line. For labor, this represents the number of hours.");
        entity.Property(e => e.UnitCost)
            .HasColumnType("numeric(18,4)")
            .HasDefaultValue(0m)
            .HasComment("Cost per unit, hour, or other quantity basis for the invoice line.");
        entity.Property(e => e.ExtendedCost)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment(
                "Total cost of the invoice line calculated from the applicable quantity and unit cost.");
        entity.Property(e => e.UnitPrice)
            .HasColumnType("numeric(18,4)")
            .HasDefaultValue(0m)
            .HasComment("Sales price per unit, hour, or other quantity basis for the invoice line.");
        entity.Property(e => e.ExtendedPrice)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment(
                "Total sales price of the invoice line calculated from the applicable quantity and unit price.");
        entity.Property(e => e.IsTaxable)
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether the invoice line is subject to applicable sales tax calculation.");
        entity.Property(e => e.GLBreak1Id)
            .HasComment(
                "Identifies the first general ledger break or accounting classification assigned to the invoice line.");
        entity.Property(e => e.GLBreak2Id)
            .HasComment(
                "Identifies the second general ledger break or accounting classification assigned to the invoice line.");
        entity.Property(e => e.LineAddedFrom)
            .HasMaxLength(50)
            .HasComment(
                "Identifies the type of source document or transaction from which the invoice line was added, such as an Estimate or Work Order.");
        entity.Property(e => e.LineAddedFromId)
            .HasComment("Identifies the specific source record from which the invoice line was added.");
        entity.Property(e => e.AddedSource)
            .HasMaxLength(50)
            .HasComment(
                "Identifies the source or mechanism through which the invoice line was added to the invoice.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the invoice detail line was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("Identifies the user who created the invoice detail line.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamp")
            .HasComment("Date and time when the invoice detail line was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("Identifies the user who last updated the invoice detail line.");

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
