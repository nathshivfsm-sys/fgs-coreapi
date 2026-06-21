using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;

namespace Fgs.Setup.Infrastructure.SetupLaborRateTypes;

internal sealed class FgsSetupLaborRateTypeSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSetupLaborRateTypeSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Name,
            Description,
            SortOrder,
            IsSystem,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSetupLaborRateTypeDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSetupLaborRateTypeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Name,
            Description,
            SortOrder,
            IsSystem,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSetupLaborRateTypeLookupRow
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }

    public FgsSetupLaborRateTypeLookupDto ToDto() => new(Id,
            Name,
            SortOrder);
}
