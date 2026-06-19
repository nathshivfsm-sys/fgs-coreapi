using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Domain.Entities;
using Fgs.Persistence.Abstractions;
using MediatR;

namespace Fgs.File.Application.Features.Files.Queries.GetFileById;

public sealed class GetFileByIdQueryHandler(IUnitOfWork unitOfWork)
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
            file.CreatedOn));
    }
}
