namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global default time slots used for onboarding seed data into setup.FgsSetupTimeSlot.
/// </summary>
public class GloTimeSlot : GloEntityBase
{
    public short Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public TimeSpan BeginTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public TimeSpan? MarkTechArrivedLateAfter { get; set; }

    public TimeSpan? MarkWorkOrderDelayedCompletionAfter { get; set; }

    public bool IsMobileVisible { get; set; } = true;

    public bool IsCustomerPortalVisible { get; set; } = true;

    public bool IncludeInCapacityPlanning { get; set; }

    public bool ShowToExternalSystem { get; set; }
}
