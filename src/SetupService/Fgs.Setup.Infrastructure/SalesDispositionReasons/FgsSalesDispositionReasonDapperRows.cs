using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;

namespace Fgs.Setup.Infrastructure.SalesDispositionReasons;

internal sealed class FgsSalesDispositionReasonSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string DispositionReasonCode { get; set; }
    public string DispositionReasonName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool RequireComment { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsSalesDispositionReasonSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            DispositionReasonCode,
            DispositionReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            RequireComment,
            IsTerminal,
            AllowManualSelection,
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsSalesDispositionReasonDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string DispositionReasonCode { get; set; }
    public string DispositionReasonName { get; set; }
    public string? Description { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool AppliesToLead { get; set; }
    public bool AppliesToOpportunity { get; set; }
    public bool RequireComment { get; set; }
    public bool IsTerminal { get; set; }
    public bool AllowManualSelection { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsSalesDispositionReasonDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            DispositionReasonCode,
            DispositionReasonName,
            Description,
            DisplayOrder,
            IsSystem,
            AppliesToLead,
            AppliesToOpportunity,
            RequireComment,
            IsTerminal,
            AllowManualSelection,
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsSalesDispositionReasonLookupRow
{
    public long Id { get; set; }
    public string DispositionReasonCode { get; set; }
    public string DispositionReasonName { get; set; }
    public short DisplayOrder { get; set; }

    public FgsSalesDispositionReasonLookupDto ToDto() => new(Id,
            DispositionReasonCode,
            DispositionReasonName,
            DisplayOrder);
}
