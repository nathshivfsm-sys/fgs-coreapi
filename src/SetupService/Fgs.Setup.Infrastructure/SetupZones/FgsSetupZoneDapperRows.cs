using Fgs.Setup.Application.Features.SetupZones.Dtos;

namespace Fgs.Setup.Infrastructure.SetupZones;

internal sealed class FgsSetupZoneSummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupZoneSummaryDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            Description,
            IsActive);
}

internal sealed class FgsSetupZoneDetailRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupZoneDetailDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            Description,
            IsActive);
}

internal sealed class FgsSetupZoneLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public FgsSetupZoneLookupDto ToDto() => new(Id,
            Code,
            Name);
}
