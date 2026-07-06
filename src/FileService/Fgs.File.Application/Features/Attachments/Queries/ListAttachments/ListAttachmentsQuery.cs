using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.ListAttachments;

public sealed record ListAttachmentsQuery(AttachmentListQuery Query, AttachmentListFilters Filters)
    : IRequest<ApiResponse<PagedResult<AttachmentMetadataDto>>>;
