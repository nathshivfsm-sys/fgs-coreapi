using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.UpdateFgsSetupDescription;

public sealed class UpdateFgsSetupDescriptionCommandHandler(
    IFgsSetupDescriptionWriteService writeService,
    ILogger<UpdateFgsSetupDescriptionCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupDescriptionCommand, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        UpdateFgsSetupDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated setup description {Id}", result.Id);
            return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update setup description {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupDescriptionDetailDto>(ex);
        }
    }
}
