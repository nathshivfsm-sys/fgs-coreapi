using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.DeleteResolutionCode;

public sealed class DeleteResolutionCodeCommandHandler(
    IResolutionCodeWriteService writeService,
    ILogger<DeleteResolutionCodeCommandHandler> logger)
    : IRequestHandler<DeleteResolutionCodeCommand, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        DeleteResolutionCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted resolution code {Id}", result.Id);
            return ApiResponse<ResolutionCodeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete resolution code {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<ResolutionCodeDetailDto>(ex);
        }
    }
}
