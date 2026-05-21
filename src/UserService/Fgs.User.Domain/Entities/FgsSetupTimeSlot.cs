namespace Fgs.User.Domain.Entities;

public class FgsSetupTimeSlot : FgsTenantCompanySetupEntityBase<long>
{
    public long? FgsSetupZoneId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public TimeSpan BeginTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public TimeSpan? MarkTechArrivedLateAfter { get; set; }

    public TimeSpan? MarkWorkOrderDelayedCompletionAfter { get; set; }

    public bool IsMobileVisible { get; set; } = true;

    public bool IsCustomerPortalVisible { get; set; } = true;
}
