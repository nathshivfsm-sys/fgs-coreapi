using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.SetupPriceBookVersions;

internal sealed class FgsSetupPriceBookVersionSummaryRow
{
    public long Id { get; set; }

    public string Version { get; set; } = null!;

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsActive { get; set; }

    public FgsSetupPriceBookVersionSummaryDto ToDto() =>
        new(Id, Version, EffectiveFrom, EffectiveTo, IsActive);
}

internal sealed class FgsSetupPriceBookVersionDetailRow
{
    public long Id { get; set; }

    public string Version { get; set; } = null!;

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsActive { get; set; }

    public FgsSetupPriceBookVersionDetailDto ToDto() =>
        new(Id, Version, EffectiveFrom, EffectiveTo, IsActive);
}

internal sealed class FgsSetupPriceBookVersionLookupRow
{
    public long Id { get; set; }

    public string Version { get; set; } = null!;

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public FgsSetupPriceBookVersionLookupDto ToDto() =>
        new(Id, Version, EffectiveFrom, EffectiveTo);
}
