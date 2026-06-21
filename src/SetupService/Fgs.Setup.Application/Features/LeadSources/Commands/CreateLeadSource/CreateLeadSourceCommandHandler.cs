using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.CreateLeadSource;

public sealed class CreateLeadSourceCommandHandler(
    ILeadSourceWriteService writeService,
    ILogger<CreateLeadSourceCommandHandler> logger)
    : IRequestHandler<CreateLeadSourceCommand, ApiResponse<LeadSourceDetailDto>>
{
    public async Task<ApiResponse<LeadSourceDetailDto>> Handle(
        CreateLeadSourceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created lead source {Id} with code {SourceCode}", result.Id, result.SourceCode);
            return ApiResponse<LeadSourceDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create lead source");
            return CatalogCrudExceptionMapper.MapException<LeadSourceDetailDto>(ex);
        }
    }
}
