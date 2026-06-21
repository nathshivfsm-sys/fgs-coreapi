using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.UpdateLeadStatus;

public sealed class UpdateLeadStatusCommandHandler(
    ILeadStatusWriteService writeService,
    ILogger<UpdateLeadStatusCommandHandler> logger)
    : IRequestHandler<UpdateLeadStatusCommand, ApiResponse<LeadStatusDetailDto>>
{
    public async Task<ApiResponse<LeadStatusDetailDto>> Handle(
        UpdateLeadStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated lead status {Id}", result.Id);
            return ApiResponse<LeadStatusDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update lead status {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<LeadStatusDetailDto>(ex);
        }
    }
}
