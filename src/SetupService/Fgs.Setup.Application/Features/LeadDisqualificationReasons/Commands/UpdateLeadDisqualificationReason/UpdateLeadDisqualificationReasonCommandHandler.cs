using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.UpdateLeadDisqualificationReason;

public sealed class UpdateLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ILogger<UpdateLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<UpdateLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        UpdateLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated lead disqualification reason {Id}", result.Id);
            return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update lead disqualification reason {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<LeadDisqualificationReasonDetailDto>(ex);
        }
    }
}
