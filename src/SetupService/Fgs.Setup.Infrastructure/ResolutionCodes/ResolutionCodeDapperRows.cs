using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

namespace Fgs.Setup.Infrastructure.ResolutionCodes;

internal sealed class ResolutionCodeSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public int GloResolutionTypeId { get; set; }
    public string ResolutionCode { get; set; }
    public string ResolutionName { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public ResolutionCodeSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            GloResolutionTypeId,
            ResolutionCode,
            ResolutionName,
            IsMobileVisible,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class ResolutionCodeDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public int GloResolutionTypeId { get; set; }
    public string ResolutionCode { get; set; }
    public string ResolutionName { get; set; }
    public bool IsMobileVisible { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public ResolutionCodeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            GloResolutionTypeId,
            ResolutionCode,
            ResolutionName,
            IsMobileVisible,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class ResolutionCodeLookupRow
{
    public long Id { get; set; }
    public string ResolutionCode { get; set; }
    public string ResolutionName { get; set; }

    public ResolutionCodeLookupDto ToDto() => new(Id,
            ResolutionCode,
            ResolutionName);
}
