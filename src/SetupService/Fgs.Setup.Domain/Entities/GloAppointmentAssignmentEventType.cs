using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global catalog of technician appointment assignment event types.
/// </summary>
public class GloAppointmentAssignmentEventType : GloEntityBase
{
    public short EventTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
