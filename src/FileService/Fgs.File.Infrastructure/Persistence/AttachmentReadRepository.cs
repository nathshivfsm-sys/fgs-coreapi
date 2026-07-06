using Fgs.File.Application.Abstractions.Persistence;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using Fgs.File.Application.Features.Attachments;
using Fgs.File.Domain.Entities;
using Fgs.File.Infrastructure.Database;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.File.Infrastructure.Persistence;

internal sealed class AttachmentReadRepository(
    FgsFileDbContext dbContext,
    ITenantContextAccessor tenantContextAccessor,
    IAttachmentUrlBuilder urlBuilder) : IAttachmentReadRepository
{
    public async Task<PagedResult<AttachmentMetadataDto>> ListAsync(
        AttachmentListQuery query,
        AttachmentListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var tenantContext = tenantContextAccessor.Current
            ?? throw new InvalidOperationException("Tenant context is not resolved.");

        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.FgsFiles
            .AsNoTracking()
            .Where(f => f.TenantId == tenantContext.TenantId && f.CompanyId == tenantContext.CompanyId);

        if (filters.IsVisibleToCustomer.HasValue)
        {
            dbQuery = dbQuery.Where(f => f.IsVisibleToCustomer == filters.IsVisibleToCustomer.Value);
        }

        if (filters.IsVisibleToFieldTechnician.HasValue)
        {
            dbQuery = dbQuery.Where(f => f.IsVisibleToFieldTechnician == filters.IsVisibleToFieldTechnician.Value);
        }

        if (!string.IsNullOrWhiteSpace(filters.EntityType))
        {
            dbQuery = dbQuery.Where(f => f.EntityType == filters.EntityType);
        }

        if (filters.EntityId.HasValue)
        {
            dbQuery = dbQuery.Where(f => f.EntityId == filters.EntityId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filters.Category))
        {
            var categoryTag = AttachmentCategoryTags.ToTag(filters.Category);
            dbQuery = dbQuery.Where(f => f.Tags != null && f.Tags.Contains(categoryTag));
        }

        if (!string.IsNullOrWhiteSpace(filters.ContentType))
        {
            dbQuery = dbQuery.Where(f => f.ContentType == filters.ContentType);
        }

        if (!string.IsNullOrWhiteSpace(filters.Extension))
        {
            dbQuery = dbQuery.Where(f => f.FileExtension == filters.Extension);
        }

        if (!string.IsNullOrWhiteSpace(filters.FileName))
        {
            var pattern = $"%{filters.FileName}%";
            dbQuery = dbQuery.Where(f => EF.Functions.ILike(f.OriginalFileName, pattern));
        }

        if (!string.IsNullOrWhiteSpace(filters.UploadedBy))
        {
            var pattern = $"%{filters.UploadedBy}%";
            dbQuery = dbQuery.Where(f => EF.Functions.ILike(f.UploadedByName, pattern));
        }

        if (filters.UploadedByUserId.HasValue)
        {
            dbQuery = dbQuery.Where(f => f.UploadedByUserId == filters.UploadedByUserId.Value);
        }

        if (filters.Tags is { Length: > 0 })
        {
            foreach (var tag in filters.Tags.Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                var trimmed = tag.Trim();
                dbQuery = dbQuery.Where(f => f.Tags != null && f.Tags.Contains(trimmed));
            }
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            var pattern = $"%{paging.Search}%";
            dbQuery = dbQuery.Where(f =>
                EF.Functions.ILike(f.OriginalFileName, pattern)
                || (f.Description != null && EF.Functions.ILike(f.Description, pattern)));
        }

        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var items = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = items.Select(f => AttachmentMetadataMapper.ToDto(f, urlBuilder)).ToList();
        return new PagedResult<AttachmentMetadataDto>(dtos, page, pageSize, totalCount);
    }

    private static IQueryable<FgsFile> ApplySort(
        IQueryable<FgsFile> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        return (sortBy?.ToLowerInvariant()) switch
        {
            "id" => desc ? query.OrderByDescending(f => f.Id) : query.OrderBy(f => f.Id),
            "originalfilename" => desc ? query.OrderByDescending(f => f.OriginalFileName) : query.OrderBy(f => f.OriginalFileName),
            "storedfilename" => desc ? query.OrderByDescending(f => f.StoredFileName) : query.OrderBy(f => f.StoredFileName),
            "contenttype" => desc ? query.OrderByDescending(f => f.ContentType) : query.OrderBy(f => f.ContentType),
            "fileextension" => desc ? query.OrderByDescending(f => f.FileExtension) : query.OrderBy(f => f.FileExtension),
            "filesizebytes" => desc ? query.OrderByDescending(f => f.FileSizeBytes) : query.OrderBy(f => f.FileSizeBytes),
            "entitytype" => desc ? query.OrderByDescending(f => f.EntityType) : query.OrderBy(f => f.EntityType),
            "entityid" => desc ? query.OrderByDescending(f => f.EntityId) : query.OrderBy(f => f.EntityId),
            "uploadedbyname" => desc ? query.OrderByDescending(f => f.UploadedByName) : query.OrderBy(f => f.UploadedByName),
            "isvisibletocustomer" => desc ? query.OrderByDescending(f => f.IsVisibleToCustomer) : query.OrderBy(f => f.IsVisibleToCustomer),
            "isvisibletofieldtechnician" => desc ? query.OrderByDescending(f => f.IsVisibleToFieldTechnician) : query.OrderBy(f => f.IsVisibleToFieldTechnician),
            "createdon" => desc ? query.OrderByDescending(f => f.CreatedOn) : query.OrderBy(f => f.CreatedOn),
            "createdby" => desc ? query.OrderByDescending(f => f.CreatedBy) : query.OrderBy(f => f.CreatedBy),
            "updatedon" => desc ? query.OrderByDescending(f => f.UpdatedOn) : query.OrderBy(f => f.UpdatedOn),
            "updatedby" => desc ? query.OrderByDescending(f => f.UpdatedBy) : query.OrderBy(f => f.UpdatedBy),
            "uploadedon" => desc ? query.OrderByDescending(f => f.CreatedOn) : query.OrderBy(f => f.CreatedOn),
            _ => desc ? query.OrderByDescending(f => f.Id) : query.OrderBy(f => f.Id)
        };
    }
}
