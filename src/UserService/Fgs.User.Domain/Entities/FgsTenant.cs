namespace Fgs.User.Domain.Entities;

public class FgsTenant : FgsEntityBase
{
    public long Id { get; set; }

    public short FgsTenantStatusId { get; set; } = 1;

    public string TenantCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Website { get; set; }

    public Guid? PhysicalLocationId { get; set; }

    public Guid? BillingLocationId { get; set; }

    public int? SubscriptionPlanId { get; set; }

    public string? TimeZone { get; set; }

    public string? DefaultCurrency { get; set; }

    public int? DefaultLanguageId { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Private S3 bucket name for tenant-wide and company assets.
    /// </summary>
    public string? StorageBucketName { get; set; }
}
