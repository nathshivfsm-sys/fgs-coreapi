using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.DeleteFgsSetupCommunicationTemplate;

public sealed class DeleteFgsSetupCommunicationTemplateCommandHandler(
    IFgsSetupCommunicationTemplateWriteService writeService,
    ILogger<DeleteFgsSetupCommunicationTemplateCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupCommunicationTemplateCommand, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        DeleteFgsSetupCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted communication template {Id}", result.Id);
            return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete communication template {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupCommunicationTemplateDetailDto>(ex);
        }
    }
}
