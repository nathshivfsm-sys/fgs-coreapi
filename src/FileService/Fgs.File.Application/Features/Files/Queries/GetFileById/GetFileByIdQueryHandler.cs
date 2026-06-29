using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Fgs.File.Domain.Entities;
using Fgs.Persistence.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.File.Application.Features.Files.Queries.GetFileById;

public sealed class GetFileByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IFileContentUrlBuilder contentUrlBuilder,
    IS3ObjectStorageService objectStorageService,
    IOptions<FileServiceOptions> fileOptions)
    : IRequestHandler<GetFileByIdQuery, ApiResponse<FileMetadataDto>>
{
    public async Task<ApiResponse<FileMetadataDto>> Handle(
        GetFileByIdQuery request,
        CancellationToken cancellationToken)
    {
        var file = await unitOfWork.Repository<FgsFile>()
            .FirstOrDefaultAsync(f => f.Id == request.FileId, cancellationToken);

        if (file is null)
        {
            return ApiResponse<FileMetadataDto>.Fail(["File not found."], ApiStatusCodes.NotFound);
        }

        string? thumbnailUrl = null;
        if (!string.IsNullOrWhiteSpace(file.ThumbnailObjectKey))
        {
            var expiry = TimeSpan.FromMinutes(fileOptions.Value.DownloadUrlExpiryMinutes);
            thumbnailUrl = await objectStorageService.CreateDownloadUrlAsync(
                file.BucketName,
                file.ThumbnailObjectKey,
                expiry,
                cancellationToken);
        }

        return ApiResponse<FileMetadataDto>.Ok(new FileMetadataDto(
            file.Id,
            file.TenantId,
            file.CompanyId,
            file.EntityType,
            file.EntityId,
            file.OriginalFileName,
            file.ContentType ?? "application/octet-stream",
            file.FileSizeBytes,
            file.Tags,
            file.Description,
            file.IsVisibleToCustomer,
            file.IsVisibleToFieldTechnician,
            contentUrlBuilder.BuildContentUrl(file.Id),
            thumbnailUrl,
            file.CreatedOn,
            file.CreatedBy,
            file.UpdatedOn,
            file.UpdatedBy));
    }
}
