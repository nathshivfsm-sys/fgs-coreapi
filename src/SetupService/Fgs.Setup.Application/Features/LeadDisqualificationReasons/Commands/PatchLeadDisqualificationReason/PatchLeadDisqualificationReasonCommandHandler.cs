using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.PatchLeadDisqualificationReason;

public sealed class PatchLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ILogger<PatchLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<PatchLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        PatchLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patchd lead disqualification reason {Id}", result.Id);
            return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to patch lead disqualification reason {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<LeadDisqualificationReasonDetailDto>(ex);
        }
    }
}
