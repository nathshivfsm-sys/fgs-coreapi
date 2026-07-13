namespace Fgs.User.Domain.Entities;

/// <summary>
/// Master catalog of public API events that external applications may subscribe to through webhooks.
/// </summary>
public class FgsApiEvent
{
    public long Id { get; set; }

    public string EventCode { get; set; } = null!;

    public string EventCategory { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short EventVersion { get; set; } = 1;

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
