using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;

namespace Fgs.Setup.Infrastructure.FgsBusinessTypes;

internal sealed class FgsBusinessTypeSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsBusinessTypeSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            Description,
            DisplayOrder,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsBusinessTypeDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsBusinessTypeDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            Code,
            Name,
            Description,
            DisplayOrder,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsBusinessTypeLookupRow
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public short? DisplayOrder { get; set; }

    public FgsBusinessTypeLookupDto ToDto() => new(Id,
            Code,
            Name,
            DisplayOrder);
}
