using Fgs.Setup.Application.Features.SetupZones.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupZones;

internal sealed class FgsSetupZoneSummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
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
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
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
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsSetupZoneLookupDto ToDto() => new(Id,
            Code,
            Name);
}
