using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Tags.Commands.UpdateFgsTag;

public sealed class UpdateFgsTagCommandHandler(
    IFgsTagWriteService writeService,
    ILogger<UpdateFgsTagCommandHandler> logger)
    : IRequestHandler<UpdateFgsTagCommand, ApiResponse<FgsTagDetailDto>>
{
    public async Task<ApiResponse<FgsTagDetailDto>> Handle(
        UpdateFgsTagCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated tag {Id}", result.Id);
            return ApiResponse<FgsTagDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update tag {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsTagDetailDto>(ex);
        }
    }
}
