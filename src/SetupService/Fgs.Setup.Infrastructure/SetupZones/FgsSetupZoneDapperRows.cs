using Fgs.Setup.Application.Features.SetupZones.Dtos;

namespace Fgs.Setup.Infrastructure.SetupZones;

internal sealed class FgsSetupZoneSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupZoneSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            Description,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupZoneDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupZoneDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            Description,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
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
