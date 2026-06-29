using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Persistence;
using Fgs.File.Application.Features.Attachments;
using Fgs.File.Application.Features.Attachments.Queries.ListAttachments;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.ListAttachments;

public sealed class ListAttachmentsQueryHandler(IAttachmentReadRepository readRepository)
    : IRequestHandler<ListAttachmentsQuery, ApiResponse<PagedResult<AttachmentMetadataDto>>>
{
    public async Task<ApiResponse<PagedResult<AttachmentMetadataDto>>> Handle(
        ListAttachmentsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<AttachmentMetadataDto>>.Ok(result);
    }
}
