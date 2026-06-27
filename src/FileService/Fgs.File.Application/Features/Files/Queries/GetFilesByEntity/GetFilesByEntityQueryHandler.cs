using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common;
using Fgs.File.Domain.Entities;
using Fgs.Persistence.Abstractions;
using MediatR;

namespace Fgs.File.Application.Features.Files.Queries.GetFilesByEntity;

public sealed class GetFilesByEntityQueryHandler(
    IUnitOfWork unitOfWork,
    IFileContentUrlBuilder contentUrlBuilder)
    : IRequestHandler<GetFilesByEntityQuery, ApiResponse<CompanyLogoDto>>
{
    public async Task<ApiResponse<CompanyLogoDto>> Handle(
        GetFilesByEntityQuery request,
        CancellationToken cancellationToken)
    {
        if (!FileEntityTypes.TryParse(request.EntityType, out var entityType))
        {
            return ApiResponse<CompanyLogoDto>.Fail(
                ["Unsupported entity type."],
                ApiStatusCodes.BadRequest);
        }

        var entityTypeValue = FileEntityTypes.ToStorageValue(entityType);
        var files = await unitOfWork.Repository<FgsFile>().ListAsync(
            file => file.EntityType == entityTypeValue && file.EntityId == request.EntityId,
            cancellationToken);

        var logoFiles = files
            .Where(FileUploadState.IsCompleted)
            .Where(file => file.Tags != null && file.Tags.Contains(FileLogoVariants.LogoTag))
            .ToList();

        var variants = new Dictionary<string, FileVariantInfoDto?>(StringComparer.OrdinalIgnoreCase);
        foreach (var variant in FileLogoVariants.SupportedVariants)
        {
            var match = logoFiles
                .Where(file => file.Tags!.Contains(variant, StringComparer.OrdinalIgnoreCase))
                .OrderByDescending(file => file.CreatedOn)
                .FirstOrDefault();

            variants[variant] = match is null
                ? null
                : new FileVariantInfoDto(
                    match.Id,
                    contentUrlBuilder.BuildContentUrl(match.Id),
                    null,
                    match.ContentType ?? "application/octet-stream",
                    match.FileSizeBytes);
        }

        return ApiResponse<CompanyLogoDto>.Ok(new CompanyLogoDto(
            entityTypeValue,
            request.EntityId,
            variants));
    }
}
