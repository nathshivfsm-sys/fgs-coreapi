using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Tags.Commands.DeleteFgsTag;

public sealed class DeleteFgsTagCommandHandler(
    IFgsTagWriteService writeService,
    ILogger<DeleteFgsTagCommandHandler> logger)
    : IRequestHandler<DeleteFgsTagCommand, ApiResponse<FgsTagDetailDto>>
{
    public async Task<ApiResponse<FgsTagDetailDto>> Handle(
        DeleteFgsTagCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted tag {Id}", result.Id);
            return ApiResponse<FgsTagDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete tag {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsTagDetailDto>(ex);
        }
    }
}
