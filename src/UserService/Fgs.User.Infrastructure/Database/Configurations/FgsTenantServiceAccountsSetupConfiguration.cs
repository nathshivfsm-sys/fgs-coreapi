using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsTenantServiceAccountsSetupConfiguration : IEntityTypeConfiguration<FgsTenantServiceAccountsSetup>
{
    public void Configure(EntityTypeBuilder<FgsTenantServiceAccountsSetup> entity)
    {
        entity.ToTable("FgsTenantServiceAccountsSetup");
        entity.HasKey(e => new { e.TenantId, e.CompanyId });
        entity.Property(e => e.TenantId).HasColumnOrder(0);
        entity.Property(e => e.CompanyId).HasColumnOrder(1);
        entity.Property(e => e.BankAccountId)
            .HasComment("Default bank account used for customer payments, deposits, and cash transactions.");
        entity.Property(e => e.AccountsReceivableAccountId)
            .HasComment("General ledger account used to record customer accounts receivable.");
        entity.Property(e => e.RevenueAccountId)
            .HasComment("Default revenue or income account used when posting invoices and completed work orders.");
        entity.Property(e => e.DiscountAccountId)
            .HasComment("General ledger account used to record customer discounts and promotional adjustments.");
        entity.Property(e => e.SalesTaxPayableAccountId)
            .HasComment("Liability account used to record collected sales taxes owed to tax authorities.");
        entity.Property(e => e.InventoryAccountId)
            .HasComment("Asset account used to record the value of inventory on hand.");
        entity.Property(e => e.COGSAccountId)
            .HasComment("Cost of Goods Sold account used when inventory is consumed or sold.");
        entity.Property(e => e.UndepositedFundsAccountId)
            .HasComment("Holding account used for customer payments received but not yet deposited into a bank account.");
        entity.Property(e => e.ProcessingFeeAccountId)
            .HasComment("Expense account used to record merchant, credit card, and payment processing fees.");
        entity.Property(e => e.AccountsPayableAccountId)
            .HasComment("General ledger account used to record amounts owed to vendors and suppliers.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(c => new { c.TenantId, c.CompanyNumber })
            .HasConstraintName("FK_FgsTenantServiceAccountsSetup_TenantCompany")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
