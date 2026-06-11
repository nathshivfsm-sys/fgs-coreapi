using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmLeadConfiguration : IEntityTypeConfiguration<CrmLead>
{
    public void Configure(EntityTypeBuilder<CrmLead> entity)
    {
        entity.ToTable(
            "CrmLead",
            t => t.HasComment(
                "Stores inbound sales inquiries and prospects prior to qualification and conversion into customers and opportunities."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.LeadStatusId).HasComment("Current lead status selected from setup.FgsLeadStatus.");
        entity.Property(e => e.LeadSourceId).HasComment("Source that generated the lead selected from setup.FgsLeadSource.");
        entity.Property(e => e.CampaignId).HasComment("Marketing campaign associated with the lead.");
        entity.Property(e => e.LeadSummary).HasMaxLength(255).IsRequired().HasComment("Short summary describing the lead inquiry.");
        entity.Property(e => e.LeadDescription).HasColumnType("text").HasComment("Detailed description of the lead inquiry and customer requirements.");
        entity.Property(e => e.FirstName).HasMaxLength(100).HasComment("Lead contact first name.");
        entity.Property(e => e.LastName).HasMaxLength(100).HasComment("Lead contact last name.");
        entity.Property(e => e.CompanyName).HasMaxLength(200).HasComment("Company or organization associated with the lead.");
        entity.Property(e => e.CustomerTypeId).HasComment("Customer type associated with the lead.");
        entity.Property(e => e.Email).HasMaxLength(255).HasComment("Primary email address for the lead.");
        entity.Property(e => e.Phone).HasMaxLength(50).HasComment("Primary phone number for the lead.");
        entity.Property(e => e.PrimaryContactMethodId).HasComment("Preferred or originating contact method for the lead.");
        entity.Property(e => e.ServiceZipCode).HasMaxLength(20).HasComment("ZIP or postal code where service is requested.");
        entity.Property(e => e.AssignedToUserId).HasComment("User assigned to work the lead.");
        entity.Property(e => e.LeadReceivedOn).HasComment("Date and time the lead was originally received.");
        entity.Property(e => e.QualifiedOn).HasComment("Date and time the lead was qualified.");
        entity.Property(e => e.DisqualifiedOn).HasComment("Date and time the lead was disqualified.");
        entity.Property(e => e.DisqualificationReasonId).HasComment("Reason the lead was disqualified selected from setup.FgsLeadDisqualificationReason.");
        entity.Property(e => e.CustomerId).HasComment("Customer record created from this lead after conversion.");
        entity.Property(e => e.ConvertedOn).HasComment("Date and time the lead was converted into a customer.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadStatusId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_LeadStatusId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadSourceId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_LeadSourceId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CampaignId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_CampaignId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerTypeId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_CustomerTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PrimaryContactMethodId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_PrimaryContactMethodId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssignedToUserId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_AssignedToUserId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisqualificationReasonId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadReceivedOn }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_LeadReceivedOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceZipCode }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_ServiceZipCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId_CustomerId");
    }
}
