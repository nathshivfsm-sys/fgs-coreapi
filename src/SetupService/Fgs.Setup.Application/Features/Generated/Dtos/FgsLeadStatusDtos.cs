namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsLeadStatus</summary>
public sealed record FgsLeadStatusSummaryDto(
    /// <summary>Identifier of the tenant that owns the lead status.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>Identifier of the company that owns the lead status.</summary>
    long CompanyId,
    /// <summary>Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the lead status.</summary>
    string? Description,
    /// <summary>Determines the order in which statuses appear in dropdowns, lists, and reports.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the record was seeded by the system or created by a user.</summary>
    bool IsSystem,
    /// <summary>Date and time when the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time when the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the status is available for selection and use.</summary>
    bool IsActive)
;

public sealed record FgsLeadStatusDetailDto(
    /// <summary>Identifier of the tenant that owns the lead status.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>Identifier of the company that owns the lead status.</summary>
    long CompanyId,
    /// <summary>Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the lead status.</summary>
    string? Description,
    /// <summary>Determines the order in which statuses appear in dropdowns, lists, and reports.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the record was seeded by the system or created by a user.</summary>
    bool IsSystem,
    /// <summary>Date and time when the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time when the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the status is available for selection and use.</summary>
    bool IsActive)
;

public sealed record FgsLeadStatusCreateDto(
    /// <summary>Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the lead status.</summary>
    string? Description,
    /// <summary>Determines the order in which statuses appear in dropdowns, lists, and reports.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the record was seeded by the system or created by a user.</summary>
    bool IsSystem)
;

public sealed record FgsLeadStatusUpdateDto(
    /// <summary>Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the lead status.</summary>
    string? Description,
    /// <summary>Determines the order in which statuses appear in dropdowns, lists, and reports.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the record was seeded by the system or created by a user.</summary>
    bool IsSystem)
;

public sealed record FgsLeadStatusPatchDto(
    /// <summary>Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED.</summary>
    string? StatusCode,
    /// <summary>User-friendly name displayed throughout the application.</summary>
    string? StatusName,
    /// <summary>Optional description explaining the purpose of the lead status.</summary>
    string? Description,
    /// <summary>Determines the order in which statuses appear in dropdowns, lists, and reports.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the record was seeded by the system or created by a user.</summary>
    bool? IsSystem)
;

