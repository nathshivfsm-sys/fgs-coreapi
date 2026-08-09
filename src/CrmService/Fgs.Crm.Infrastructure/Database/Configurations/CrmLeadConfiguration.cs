using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmLeadConfiguration : IEntityTypeConfiguration<CrmLead>
{
    public void Configure(EntityTypeBuilder<CrmLead> entity)
    {
        entity.ToTable(
            "CrmLead",
            t => t.HasComment(
                "Stores sales leads/prospects received from the website, office users, technicians, referrals, campaigns, or other configured lead sources. A Lead may remain in the Lead pipeline, be disqualified/lost, be associated with an existing customer, or be converted into an Opportunity. Lead activities are stored separately in crm.FgsSalesActivity."));

        entity.HasKey(e => e.Id).HasName("PK_CrmLead");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.LeadStatusId)
            .HasComment("Current status of the lead selected from the configured sales pipeline statuses applicable to leads.");
        entity.Property(e => e.LeadSourceId)
            .HasComment("Source that generated the lead selected from setup.FgsLeadSource.");
        entity.Property(e => e.CampaignId)
            .HasComment("Marketing campaign associated with the lead.");
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired()
            .HasComment("Name of the person or contact submitting or associated with the lead.");
        entity.Property(e => e.LeadDescription).HasColumnType("text")
            .HasComment("Comments and details describing the customer inquiry, service need, or information provided with the lead.");
        entity.Property(e => e.Email).HasMaxLength(255)
            .HasComment("Primary email address for the lead.");
        entity.Property(e => e.Phone).HasMaxLength(50)
            .HasComment("Primary phone number for the lead.");
        entity.Property(e => e.PrimaryContactMethodId)
            .HasComment("Preferred or originating contact method for the lead.");
        entity.Property(e => e.Address1).HasMaxLength(200)
            .HasComment("Primary street address where service is requested.");
        entity.Property(e => e.Address2).HasMaxLength(200)
            .HasComment("Additional address information such as apartment, suite, unit, building, or floor.");
        entity.Property(e => e.City).HasMaxLength(100)
            .HasComment("City where service is requested.");
        entity.Property(e => e.State).HasMaxLength(100)
            .HasComment("State, province, or administrative region where service is requested.");
        entity.Property(e => e.PostalCode).HasMaxLength(20)
            .HasComment("Postal or ZIP code where service is requested.");
        entity.Property(e => e.Country).HasMaxLength(100)
            .HasComment("Country where service is requested.");
        entity.Property(e => e.AssignedToUserId)
            .HasComment("User assigned to work the lead.");
        entity.Property(e => e.CustomerId)
            .HasComment("Existing customer associated with the lead, when applicable.");
        entity.Property(e => e.ServiceLocationId)
            .HasComment("Optional service location associated with the lead.");
        entity.Property(e => e.LeadReceivedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the lead was originally received.");
        entity.Property(e => e.DisqualificationReasonId)
            .HasComment("Reason the lead was disqualified selected from setup.FgsLeadDisqualificationReason.");
        entity.Property(e => e.DisqualifiedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the lead was disqualified.");
        entity.Property(e => e.ConvertedOpportunityId)
            .HasComment("Opportunity created when the lead was converted.");
        entity.Property(e => e.ConvertedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the lead was converted into an opportunity.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_CrmLead_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadStatusId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_LeadStatusId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadSourceId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_LeadSourceId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CampaignId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_CampaignId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssignedToUserId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_AssignedToUserId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_CustomerId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_ServiceLocationId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisqualificationReasonId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadReceivedOn })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_LeadReceivedOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ConvertedOpportunityId })
            .HasDatabaseName("IX_CrmLead_TenantId_CompanyId_ConvertedOpportunityId");
    }
}
