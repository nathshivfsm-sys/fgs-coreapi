using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.UpdateFgsSetupCommunicationTemplate;

public sealed class UpdateFgsSetupCommunicationTemplateCommandHandler(
    IFgsSetupCommunicationTemplateWriteService writeService,
    ILogger<UpdateFgsSetupCommunicationTemplateCommandHandler> logger)
    : IRequestHandler<UpdateFgsSetupCommunicationTemplateCommand, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        UpdateFgsSetupCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated communication template {Id}", result.Id);
            return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update communication template {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsSetupCommunicationTemplateDetailDto>(ex);
        }
    }
}
