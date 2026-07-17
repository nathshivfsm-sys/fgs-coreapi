using Fgs.User.Application.Features.ApiEvents.Dtos;

namespace Fgs.User.Infrastructure.Entities.ApiEvents;

internal sealed class FgsApiEventSummaryRow
{
    public long Id { get; set; }

    public string EventCode { get; set; } = null!;

    public string EventCategory { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short EventVersion { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsApiEventSummaryDto ToDto() =>
        new(Id, EventCode, EventCategory, Name, Description, EventVersion, DisplayOrder, IsActive);
}

internal sealed class FgsApiEventDetailRow
{
    public long Id { get; set; }

    public string EventCode { get; set; } = null!;

    public string EventCategory { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short EventVersion { get; set; }

    public short DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public FgsApiEventDetailDto ToDto() =>
        new(Id, EventCode, EventCategory, Name, Description, EventVersion, DisplayOrder, IsActive);
}

internal sealed class FgsApiEventLookupRow
{
    public long Id { get; set; }

    public string EventCode { get; set; } = null!;

    public string EventCategory { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short EventVersion { get; set; }

    public short DisplayOrder { get; set; }

    public FgsApiEventLookupDto ToDto() =>
        new(Id, EventCode, EventCategory, Name, EventVersion, DisplayOrder);
}
