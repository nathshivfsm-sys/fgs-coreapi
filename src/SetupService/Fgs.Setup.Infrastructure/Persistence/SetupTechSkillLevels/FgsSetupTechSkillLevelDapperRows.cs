using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.SetupTechSkillLevels;

internal sealed class FgsSetupTechSkillLevelSummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTechSkillLevelSummaryDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            Description,
            SortOrder,
            IsActive);
}

internal sealed class FgsSetupTechSkillLevelDetailRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsSetupTechSkillLevelDetailDto ToDto() =>
        new(
            Id,
            Code,
            Name,
            Description,
            SortOrder,
            IsActive);
}

internal sealed class FgsSetupTechSkillLevelLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? SortOrder { get; set; }

    public FgsSetupTechSkillLevelLookupDto ToDto() => new(Id,
            Code,
            Name,
            SortOrder);
}
