using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsOpportunityConfiguration : IEntityTypeConfiguration<FgsOpportunity>
{
    public void Configure(EntityTypeBuilder<FgsOpportunity> entity)
    {
        entity.ToTable(
            "FgsOpportunity",
            t => t.HasComment(
                "Stores qualified sales opportunities that originate from Leads or are created directly by users. An Opportunity represents an active sales pursuit and may ultimately result in an Estimate or Work Order."));

        entity.HasKey(e => e.Id).HasName("PK_FgsOpportunity");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the opportunity.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant that owns the opportunity.");
        entity.Property(e => e.CompanyId).HasComment("Company within the tenant that owns the opportunity.");
        entity.Property(e => e.LeadId)
            .HasComment("Optional Lead from which the opportunity was created. NULL when the opportunity was created directly without a Lead.");
        entity.Property(e => e.OpportunityStatusId)
            .HasComment("Current status of the opportunity selected from the configured sales pipeline statuses applicable to opportunities.");
        entity.Property(e => e.LeadSourceId)
            .HasComment("Optional source associated with the opportunity. When the opportunity originated from a Lead, this may be copied from the Lead source.");
        entity.Property(e => e.CampaignId)
            .HasComment("Optional marketing campaign associated with the opportunity.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired()
            .HasComment("Name used to identify the sales opportunity.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Detailed description of the opportunity, customer requirements, sales information, or other relevant comments.");
        entity.Property(e => e.CustomerId)
            .HasComment("Customer associated with the opportunity. A customer is required for an active opportunity. The customer may be an existing customer or one created during Lead conversion.");
        entity.Property(e => e.ServiceLocationId)
            .HasComment("Optional service location associated with the opportunity.");
        entity.Property(e => e.AssignedToUserId)
            .HasComment("Salesperson or user currently responsible for working the opportunity.");
        entity.Property(e => e.EstimatedAmount).HasColumnType("numeric(18,2)")
            .HasComment("Current estimated sales value of the opportunity used for sales forecasting. This value may change as the opportunity progresses.");
        entity.Property(e => e.SoldAmount).HasColumnType("numeric(18,2)")
            .HasComment("Actual sales amount agreed upon when the opportunity is won. NULL until the opportunity is marked as won.");
        entity.Property(e => e.ExpectedCloseOn).HasColumnType("timestamptz")
            .HasComment("Expected date and time when the opportunity is anticipated to close.");
        entity.Property(e => e.WonOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the opportunity was marked as won.");
        entity.Property(e => e.LostOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the opportunity was marked as lost.");
        entity.Property(e => e.DispositionReasonId)
            .HasComment("Reason the opportunity was lost, selected from the configured sales disposition reasons.");
        entity.Property(e => e.EstimateId)
            .HasComment("Estimate created from the opportunity when the sales process results in an Estimate.");
        entity.Property(e => e.WorkOrderId)
            .HasComment("Work Order created from the opportunity when the sales process results directly in a Work Order.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time when the opportunity record was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the opportunity record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the opportunity record was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the opportunity record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_LeadId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.OpportunityStatusId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_OpportunityStatusId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadSourceId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_LeadSourceId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CampaignId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_CampaignId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_CustomerId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_ServiceLocationId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssignedToUserId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_AssignedToUserId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DispositionReasonId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_DispositionReasonId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ExpectedCloseOn })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_ExpectedCloseOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_EstimateId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId })
            .HasDatabaseName("IX_FgsOpportunity_TenantId_CompanyId_WorkOrderId");
    }
}
