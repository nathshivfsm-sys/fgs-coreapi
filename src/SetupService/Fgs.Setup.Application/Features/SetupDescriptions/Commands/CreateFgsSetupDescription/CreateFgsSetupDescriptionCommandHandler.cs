using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Commands.CreateFgsSetupDescription;

public sealed class CreateFgsSetupDescriptionCommandHandler(
    IFgsSetupDescriptionWriteService writeService,
    ILogger<CreateFgsSetupDescriptionCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupDescriptionCommand, ApiResponse<FgsSetupDescriptionDetailDto>>
{
    public async Task<ApiResponse<FgsSetupDescriptionDetailDto>> Handle(
        CreateFgsSetupDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created setup description {Id} with code {DescriptionTypeCode}", result.Id, result.DescriptionTypeCode);
            return ApiResponse<FgsSetupDescriptionDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create setup description");
            return CatalogCrudExceptionMapper.MapException<FgsSetupDescriptionDetailDto>(ex);
        }
    }
}
