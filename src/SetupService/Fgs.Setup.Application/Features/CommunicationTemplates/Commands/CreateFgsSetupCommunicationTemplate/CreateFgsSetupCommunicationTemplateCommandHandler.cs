using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.CreateFgsSetupCommunicationTemplate;

public sealed class CreateFgsSetupCommunicationTemplateCommandHandler(
    IFgsSetupCommunicationTemplateWriteService writeService,
    ILogger<CreateFgsSetupCommunicationTemplateCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupCommunicationTemplateCommand, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        CreateFgsSetupCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created communication template {Id} with code {Code}", result.Id, result.Code);
            return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create communication template");
            return CatalogCrudExceptionMapper.MapException<FgsSetupCommunicationTemplateDetailDto>(ex);
        }
    }
}
