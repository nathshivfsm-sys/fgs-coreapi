using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.DeleteFgsSetupDescription;

public sealed class DeleteFgsSetupDescriptionCommandHandler(
    IFgsSetupDescriptionWriteService writeService,
    ILogger<DeleteFgsSetupDescriptionCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupDescriptionCommand, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        DeleteFgsSetupDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted setup description {Id}", result.Id);
            return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete setup description {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupDescriptionDetailDto>(ex);
        }
    }
}
