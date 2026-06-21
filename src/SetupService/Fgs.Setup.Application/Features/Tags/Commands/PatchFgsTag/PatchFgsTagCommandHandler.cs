using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Tags.Commands.PatchFgsTag;

public sealed class PatchFgsTagCommandHandler(
    IFgsTagWriteService writeService,
    ILogger<PatchFgsTagCommandHandler> logger)
    : IRequestHandler<PatchFgsTagCommand, ApiResponse<FgsTagDetailDto>>
{
    public async Task<ApiResponse<FgsTagDetailDto>> Handle(
        PatchFgsTagCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd tag {Id}", result.Id);
            return ApiResponse<FgsTagDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch tag {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsTagDetailDto>(ex);
        }
    }
}
