using Fgs.Setup.Application.Features.TermsConditions.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.TermsConditions;

internal sealed class FgsTermsConditionSummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int VersionNumber { get; set; }
    public bool IsActive { get; set; }

    public FgsTermsConditionSummaryDto ToDto() =>
        new(Id, Code, Name, VersionNumber, IsActive);
}

internal sealed class FgsTermsConditionDetailRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int VersionNumber { get; set; }
    public string TermsText { get; set; } = null!;
    public bool IsActive { get; set; }

    public FgsTermsConditionDetailDto ToDto() =>
        new(Id, Code, Name, VersionNumber, TermsText, IsActive);
}

internal sealed class FgsTermsConditionLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int VersionNumber { get; set; }

    public FgsTermsConditionLookupDto ToDto() =>
        new(Id, Code, Name, VersionNumber);
}
