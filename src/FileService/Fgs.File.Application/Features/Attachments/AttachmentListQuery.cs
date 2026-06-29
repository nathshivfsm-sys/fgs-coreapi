using Fgs.Foundation.Paging;

namespace Fgs.File.Application.Features.Attachments;

public sealed record AttachmentListQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null)
{
    public PagedQuery ToPagedQuery() =>
        new(Page, PageSize, SortBy, SortDirection, Search, IsActive: null);
}

public sealed record AttachmentListFilters(
    string? EntityType = null,
    long? EntityId = null,
    bool? IsVisibleToCustomer = null,
    bool? IsVisibleToFieldTechnician = null,
    string? Category = null,
    string? ContentType = null,
    string? Extension = null,
    string? FileName = null,
    string? UploadedBy = null,
    long? UploadedByUserId = null,
    string[]? Tags = null);
