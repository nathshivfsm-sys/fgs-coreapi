using Fgs.Contracts.Clients;
using Fgs.File.Application.Features.Attachments;
using Fgs.Foundation.Paging;

namespace Fgs.File.Application.Abstractions.Persistence;

public interface IAttachmentReadRepository
{
    Task<PagedResult<AttachmentMetadataDto>> ListAsync(
        AttachmentListQuery query,
        AttachmentListFilters filters,
        CancellationToken cancellationToken = default);
}
