using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

namespace Fgs.Setup.Infrastructure.ResolutionCodes;

internal sealed class ResolutionCodeSummaryRow
{
    public long Id { get; set; }
    public int GloResolutionTypeId { get; set; }
    public string ResolutionCode { get; set; } = null!;
    public string ResolutionName { get; set; } = null!;
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }

    public ResolutionCodeSummaryDto ToDto() =>
        new(
            Id,
            GloResolutionTypeId,
            ResolutionCode,
            ResolutionName,
            IsMobileVisible,
            IsActive);
}

internal sealed class ResolutionCodeDetailRow
{
    public long Id { get; set; }
    public int GloResolutionTypeId { get; set; }
    public string ResolutionCode { get; set; } = null!;
    public string ResolutionName { get; set; } = null!;
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }

    public ResolutionCodeDetailDto ToDto() =>
        new(
            Id,
            GloResolutionTypeId,
            ResolutionCode,
            ResolutionName,
            IsMobileVisible,
            IsActive);
}

internal sealed class ResolutionCodeLookupRow
{
    public long Id { get; set; }
    public string ResolutionCode { get; set; } = null!;
    public string ResolutionName { get; set; } = null!;

    public ResolutionCodeLookupDto ToDto() => new(Id,
            ResolutionCode,
            ResolutionName);
}
