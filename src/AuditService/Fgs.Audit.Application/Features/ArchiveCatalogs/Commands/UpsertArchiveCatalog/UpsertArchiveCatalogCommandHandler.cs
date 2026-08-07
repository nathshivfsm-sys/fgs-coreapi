using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Commands.UpsertArchiveCatalog;

public sealed class UpsertArchiveCatalogCommandHandler(IArchiveCatalogWriter writer)
    : IRequestHandler<UpsertArchiveCatalogCommand, ApiResponse<ArchiveCatalogDto>>
{
    public async Task<ApiResponse<ArchiveCatalogDto>> Handle(
        UpsertArchiveCatalogCommand request,
        CancellationToken cancellationToken)
    {
        var body = request.Request;
        if (string.IsNullOrWhiteSpace(body.StoragePath))
        {
            return ApiResponse<ArchiveCatalogDto>.Fail(
                ["StoragePath is required."],
                ApiStatusCodes.BadRequest);
        }

        if (body.FileSize < 0)
        {
            return ApiResponse<ArchiveCatalogDto>.Fail(
                ["FileSize must be zero or greater."],
                ApiStatusCodes.BadRequest);
        }

        var (result, created) = await writer.UpsertAsync(body, cancellationToken);
        return ApiResponse<ArchiveCatalogDto>.Ok(
            result,
            created ? ApiStatusCodes.Created : ApiStatusCodes.Ok);
    }
}
