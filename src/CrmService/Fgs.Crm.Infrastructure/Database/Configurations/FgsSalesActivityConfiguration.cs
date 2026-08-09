using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsSalesActivityConfiguration : IEntityTypeConfiguration<FgsSalesActivity>
{
    public void Configure(EntityTypeBuilder<FgsSalesActivity> entity)
    {
        entity.ToTable(
            "FgsSalesActivity",
            t =>
            {
                t.HasComment(
                    "Stores scheduled and completed sales activities for Leads and Opportunities, including calls, emails, meetings, site visits, follow-ups, and system-generated activities. Activities can be scheduled on the dispatch board and completed with an outcome, resulting pipeline status, comments, and optional follow-up activity.");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivity_LeadOrOpportunity",
                    "(\"LeadId\" IS NOT NULL AND \"OpportunityId\" IS NULL) OR (\"LeadId\" IS NULL AND \"OpportunityId\" IS NOT NULL)");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivity_CompletedRequiresStarted",
                    "\"CompletedOn\" IS NULL OR \"StartedOn\" IS NOT NULL");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivity_CompletedAfterStarted",
                    "\"StartedOn\" IS NULL OR \"CompletedOn\" IS NULL OR \"CompletedOn\" >= \"StartedOn\"");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivity_EstimatedHours",
                    "\"EstimatedHours\" IS NULL OR \"EstimatedHours\" > 0");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivity_ActualHours",
                    "\"ActualHours\" IS NULL OR \"ActualHours\" > 0");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsSalesActivity");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the sales activity.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant that owns the sales activity.");
        entity.Property(e => e.CompanyId).HasComment("Company within the tenant that owns the sales activity.");
        entity.Property(e => e.LeadId)
            .HasComment("Lead associated with the activity. Exactly one of LeadId or OpportunityId must be populated.");
        entity.Property(e => e.OpportunityId)
            .HasComment("Opportunity associated with the activity. Exactly one of OpportunityId or LeadId must be populated.");
        entity.Property(e => e.ActivityTypeId)
            .HasComment("Activity type selected from the configured sales activity types, such as Call, Email, Visit, Meeting, or Follow-up.");
        entity.Property(e => e.AssignedToUserId)
            .HasComment("User responsible for performing the scheduled sales activity.");
        entity.Property(e => e.ScheduledOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the activity is scheduled to occur. Used to place the activity on the dispatch board.");
        entity.Property(e => e.EstimatedHours).HasColumnType("numeric(6,2)")
            .HasComment("Expected amount of time required to perform the scheduled activity, expressed in hours. Used for scheduling and dispatch capacity planning.");
        entity.Property(e => e.StartedOn).HasColumnType("timestamptz")
            .HasComment("Optional date and time when the user started performing the activity.");
        entity.Property(e => e.CompletedOn).HasColumnType("timestamptz")
            .HasComment("Optional date and time when the activity was completed. When StartedOn and CompletedOn are provided, ActualHours may be calculated from the elapsed time.");
        entity.Property(e => e.ActualHours).HasColumnType("numeric(6,2)")
            .HasComment("Actual amount of time spent performing the activity, expressed in hours. The value may be calculated from StartedOn and CompletedOn or entered directly by the user when start and completion times are not tracked.");
        entity.Property(e => e.PerformedByUserId)
            .HasComment("User who actually performed or completed the activity. This may differ from the user originally assigned to the activity.");
        entity.Property(e => e.SalesActivityOutcomeId)
            .HasComment("Outcome selected when the activity is completed. The outcome may determine the resulting pipeline status, whether another activity should be created, or whether the Lead should be converted to an Opportunity.");
        entity.Property(e => e.OutcomeDetails).HasColumnType("text")
            .HasComment("Additional details describing the selected activity outcome, including specific results, customer response, information communicated, or other details associated with the outcome.");
        entity.Property(e => e.Comments).HasColumnType("text")
            .HasComment("Comments or notes entered while scheduling, performing, or completing the activity.");
        entity.Property(e => e.RequiresFollowUp).IsRequired().HasDefaultValue(false)
            .HasComment("Indicates whether another sales activity is required after this activity.");
        entity.Property(e => e.FollowUpOn).HasColumnType("timestamptz")
            .HasComment("Date and time requested for the follow-up activity.");
        entity.Property(e => e.FollowUpActivityId)
            .HasComment("Activity created as the follow-up to this activity.");
        entity.Property(e => e.Latitude).HasColumnType("numeric(10,7)")
            .HasComment("Latitude captured when the activity is performed, when location capture is enabled.");
        entity.Property(e => e.Longitude).HasColumnType("numeric(10,7)")
            .HasComment("Longitude captured when the activity is performed, when location capture is enabled.");
        entity.Property(e => e.IsSystemGenerated).IsRequired().HasDefaultValue(false)
            .HasComment("Indicates whether the activity was created automatically by the system rather than manually by a user.");
        entity.Property(e => e.Priority).IsRequired().HasDefaultValue(SalesPriority.NORMAL)
            .HasComment("Priority of the sales activity used to indicate the urgency with which the activity should be performed.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time when the sales activity record was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the sales activity record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the sales activity record was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the sales activity record.");

        entity.HasOne<CrmLead>()
            .WithMany()
            .HasForeignKey(e => e.LeadId)
            .HasConstraintName("FK_FgsSalesActivity_Lead")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsOpportunity>()
            .WithMany()
            .HasForeignKey(e => e.OpportunityId)
            .HasConstraintName("FK_FgsSalesActivity_Opportunity")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsSalesActivity>()
            .WithMany()
            .HasForeignKey(e => e.FollowUpActivityId)
            .HasConstraintName("FK_FgsSalesActivity_FollowUpActivity")
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LeadId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_LeadId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.OpportunityId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_OpportunityId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ActivityTypeId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_ActivityTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssignedToUserId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_AssignedToUserId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ScheduledOn })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_ScheduledOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PerformedByUserId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_PerformedByUserId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SalesActivityOutcomeId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_SalesActivityOutcomeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FollowUpOn })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_FollowUpOn");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FollowUpActivityId })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_FollowUpActivityId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CompletedOn })
            .HasDatabaseName("IX_FgsSalesActivity_TenantId_CompanyId_CompletedOn");
    }
}
