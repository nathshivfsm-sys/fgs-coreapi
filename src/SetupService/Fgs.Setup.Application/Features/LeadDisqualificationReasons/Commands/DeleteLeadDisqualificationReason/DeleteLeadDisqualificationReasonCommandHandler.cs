using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.DeleteLeadDisqualificationReason;

public sealed class DeleteLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ILogger<DeleteLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<DeleteLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        DeleteLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted lead disqualification reason {Id}", result.Id);
            return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete lead disqualification reason {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<LeadDisqualificationReasonDetailDto>(ex);
        }
    }
}
