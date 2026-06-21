using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;

namespace Fgs.Setup.Infrastructure.SetupTechSkillLevels;

internal sealed class FgsSetupTechSkillLevelSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupTechSkillLevelSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            Description,
            SortOrder,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupTechSkillLevelDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupTechSkillLevelDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            Description,
            SortOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupTechSkillLevelLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? SortOrder { get; set; }

    public FgsSetupTechSkillLevelLookupDto ToDto() => new(Id,
            Code,
            Name,
            SortOrder);
}
