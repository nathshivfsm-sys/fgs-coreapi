using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.UpdateResolutionCode;

public sealed class UpdateResolutionCodeCommandHandler(
    IResolutionCodeWriteService writeService,
    ILogger<UpdateResolutionCodeCommandHandler> logger)
    : IRequestHandler<UpdateResolutionCodeCommand, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        UpdateResolutionCodeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated resolution code {Id}", result.Id);
            return ApiResponse<ResolutionCodeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update resolution code {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<ResolutionCodeDetailDto>(ex);
        }
    }
}
