using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Fgs.File.Domain.Entities;
using Fgs.Persistence.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.File.Application.Features.Files.Queries.GetFileContentUrl;

public sealed class GetFileContentUrlQueryHandler(
    IUnitOfWork unitOfWork,
    IS3ObjectStorageService objectStorageService,
    IOptions<FileServiceOptions> fileOptions)
    : IRequestHandler<GetFileContentUrlQuery, ApiResponse<FileContentUrlResponse>>
{
    public async Task<ApiResponse<FileContentUrlResponse>> Handle(
        GetFileContentUrlQuery request,
        CancellationToken cancellationToken)
    {
        var file = await unitOfWork.Repository<FgsFile>()
            .FirstOrDefaultAsync(f => f.Id == request.FileId, cancellationToken);

        if (file is null)
        {
            return ApiResponse<FileContentUrlResponse>.Fail(["File not found."], ApiStatusCodes.NotFound);
        }

        var expiry = TimeSpan.FromMinutes(fileOptions.Value.DownloadUrlExpiryMinutes);
        var expiresAt = DateTimeOffset.UtcNow.Add(expiry);
        var downloadUrl = await objectStorageService.CreateDownloadUrlAsync(
            file.BucketName,
            file.ObjectKey,
            expiry,
            cancellationToken);

        return ApiResponse<FileContentUrlResponse>.Ok(new FileContentUrlResponse(
            file.Id,
            downloadUrl,
            file.ContentType ?? "application/octet-stream",
            expiresAt));
    }
}
