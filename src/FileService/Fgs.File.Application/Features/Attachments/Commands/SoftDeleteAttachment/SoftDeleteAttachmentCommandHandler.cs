using Fgs.Contracts.Api;
using Fgs.File.Application.Common;
using Fgs.File.Application.Features.Attachments.Commands.SoftDeleteAttachment;
using Fgs.File.Domain.Entities;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Commands.SoftDeleteAttachment;

public sealed class SoftDeleteAttachmentCommandHandler(
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext)
    : IRequestHandler<SoftDeleteAttachmentCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        SoftDeleteAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is null)
        {
            return ApiResponse<object>.Fail(["Tenant context is required."], ApiStatusCodes.BadRequest);
        }

        var file = await unitOfWork.Repository<FgsFile>().GetByIdAsync(request.AttachmentId, cancellationToken);
        if (file is null
            || file.TenantId != tenantContext.TenantId
            || file.CompanyId != tenantContext.CompanyId)
        {
            return ApiResponse<object>.Fail(["Attachment not found."], ApiStatusCodes.NotFound);
        }

        if (AttachmentDeletionTags.IsDeleted(file.Tags))
        {
            return ApiResponse<object>.Ok(null!, ApiStatusCodes.NoContent);
        }

        file.Tags = AttachmentDeletionTags.MarkDeleted(file.Tags);
        file.UpdatedOn = DateTimeOffset.UtcNow;
        file.UpdatedBy = userContext.ResolveAuditActor();
        unitOfWork.Repository<FgsFile>().Update(file);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.Ok(null!, ApiStatusCodes.NoContent);
    }
}
