using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.UpdateLeadSource;

public sealed class UpdateLeadSourceCommandHandler(
    ILeadSourceWriteService writeService,
    ILogger<UpdateLeadSourceCommandHandler> logger)
    : IRequestHandler<UpdateLeadSourceCommand, ApiResponse<LeadSourceDetailDto>>
{
    public async Task<ApiResponse<LeadSourceDetailDto>> Handle(
        UpdateLeadSourceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated lead source {Id}", result.Id);
            return ApiResponse<LeadSourceDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update lead source {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<LeadSourceDetailDto>(ex);
        }
    }
}
