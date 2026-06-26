namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Standard vehicle maintenance type used when recording maintenance activities.
/// </summary>
public class GloVehicleMaintenanceType
{
    public int Id { get; set; }

    public string MaintenanceTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }
}
