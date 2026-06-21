using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.PatchResolutionCode;

public sealed class PatchResolutionCodeCommandHandler(
    IResolutionCodeWriteService writeService,
    ILogger<PatchResolutionCodeCommandHandler> logger)
    : IRequestHandler<PatchResolutionCodeCommand, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        PatchResolutionCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd resolution code {Id}", result.Id);
            return ApiResponse<ResolutionCodeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch resolution code {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<ResolutionCodeDetailDto>(ex);
        }
    }
}
