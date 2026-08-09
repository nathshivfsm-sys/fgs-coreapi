using Fgs.Setup.Application.Features.JobTypes.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypes;

internal sealed class JobTypeSummaryRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short UsedFor { get; set; }
    public string? BusinessUnit { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool ShowOnCustomerPortal { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeSummaryDto ToDto() =>
        new(
            Id,
            JobTypeCode,
            Name,
            UsedFor,
            BusinessUnit,
            BackgroundColor,
            TextColor,
            ShowToFieldTech,
            ShowOnCustomerPortal,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeDetailRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short UsedFor { get; set; }
    public string? BusinessUnit { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public bool ShowToFieldTech { get; set; }
    public bool ShowOnCustomerPortal { get; set; }
    public short? DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public JobTypeDetailDto ToDto() =>
        new(
            Id,
            JobTypeCode,
            Name,
            UsedFor,
            BusinessUnit,
            BackgroundColor,
            TextColor,
            ShowToFieldTech,
            ShowOnCustomerPortal,
            DisplayOrder,
            IsActive);
}

internal sealed class JobTypeLookupRow
{
    public long Id { get; set; }
    public string JobTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public short? DisplayOrder { get; set; }

    public JobTypeLookupDto ToDto() => new(Id,
            JobTypeCode,
            Name,
            DisplayOrder);
}
