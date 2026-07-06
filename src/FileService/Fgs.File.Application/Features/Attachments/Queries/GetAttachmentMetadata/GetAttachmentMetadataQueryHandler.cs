using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using Fgs.File.Application.Features.Attachments.Queries.GetAttachmentMetadata;
using Fgs.File.Domain.Entities;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Queries.GetAttachmentMetadata;

public sealed class GetAttachmentMetadataQueryHandler(
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IAttachmentUrlBuilder urlBuilder)
    : IRequestHandler<GetAttachmentMetadataQuery, ApiResponse<AttachmentMetadataDto>>
{
    public async Task<ApiResponse<AttachmentMetadataDto>> Handle(
        GetAttachmentMetadataQuery request,
        CancellationToken cancellationToken)
    {
        var file = await FindAttachmentAsync(request.AttachmentId, request.EntityType, cancellationToken);
        if (file is null)
        {
            return ApiResponse<AttachmentMetadataDto>.Fail(["Attachment not found."], ApiStatusCodes.NotFound);
        }

        return ApiResponse<AttachmentMetadataDto>.Ok(AttachmentMetadataMapper.ToDto(file, urlBuilder));
    }

    private async Task<FgsFile?> FindAttachmentAsync(
        long attachmentId,
        string entityType,
        CancellationToken cancellationToken)
    {
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is null)
        {
            return null;
        }

        var file = await unitOfWork.Repository<FgsFile>()
            .FirstOrDefaultAsync(f => f.Id == attachmentId, cancellationToken);

        if (file is null
            || file.TenantId != tenantContext.TenantId
            || file.CompanyId != tenantContext.CompanyId
            || !FileEntityTypes.RouteValueMatchesStorage(entityType, file.EntityType))
        {
            return null;
        }

        return file;
    }
}
