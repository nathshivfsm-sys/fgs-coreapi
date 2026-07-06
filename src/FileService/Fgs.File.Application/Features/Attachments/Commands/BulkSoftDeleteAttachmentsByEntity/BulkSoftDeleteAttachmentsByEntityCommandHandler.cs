using Fgs.Contracts.Api;
using Fgs.File.Application.Common;
using Fgs.File.Application.Features.Attachments.Commands.BulkSoftDeleteAttachmentsByEntity;
using Fgs.File.Domain.Entities;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using MediatR;

namespace Fgs.File.Application.Features.Attachments.Commands.BulkSoftDeleteAttachmentsByEntity;

public sealed class BulkSoftDeleteAttachmentsByEntityCommandHandler(
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext)
    : IRequestHandler<BulkSoftDeleteAttachmentsByEntityCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        BulkSoftDeleteAttachmentsByEntityCommand request,
        CancellationToken cancellationToken)
    {
        var tenantContext = tenantContextAccessor.Current;
        if (tenantContext is null)
        {
            return ApiResponse<object>.Fail(["Tenant context is required."], ApiStatusCodes.BadRequest);
        }

        if (!FileEntityTypes.IsSupported(request.EntityType))
        {
            return ApiResponse<object>.Fail(["Unsupported entity type."], ApiStatusCodes.BadRequest);
        }

        var entityTypeValue = FileEntityTypes.ToStorageValue(
            Enum.Parse<Domain.Enums.FileEntityType>(request.EntityType, ignoreCase: true));

        var repo = unitOfWork.Repository<FgsFile>();
        var files = await repo.ListAsync(
            f => f.TenantId == tenantContext.TenantId
                 && f.CompanyId == tenantContext.CompanyId
                 && f.EntityType == entityTypeValue
                 && f.EntityId == request.EntityId
                 && (f.Tags == null || !f.Tags.Contains(AttachmentDeletionTags.DeletedTag)),
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            var categoryTag = AttachmentCategoryTags.ToTag(request.Category);
            files = files.Where(f => f.Tags != null && f.Tags.Contains(categoryTag)).ToList();
        }

        var auditActor = userContext.ResolveAuditActor();
        var now = DateTimeOffset.UtcNow;
        foreach (var file in files)
        {
            file.Tags = AttachmentDeletionTags.MarkDeleted(file.Tags);
            file.UpdatedOn = now;
            file.UpdatedBy = auditActor;
            repo.Update(file);
        }

        if (files.Count > 0)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return ApiResponse<object>.Ok(null!, ApiStatusCodes.NoContent);
    }
}
