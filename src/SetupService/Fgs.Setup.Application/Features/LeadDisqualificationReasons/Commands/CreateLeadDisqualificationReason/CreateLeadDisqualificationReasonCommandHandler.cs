using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.CreateLeadDisqualificationReason;

public sealed class CreateLeadDisqualificationReasonCommandHandler(
    ILeadDisqualificationReasonWriteService writeService,
    ILogger<CreateLeadDisqualificationReasonCommandHandler> logger)
    : IRequestHandler<CreateLeadDisqualificationReasonCommand, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        CreateLeadDisqualificationReasonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created lead disqualification reason {Id} with code {ReasonCode}", result.Id, result.ReasonCode);
            return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create lead disqualification reason");
            return CatalogCrudExceptionMapper.MapException<LeadDisqualificationReasonDetailDto>(ex);
        }
    }
}
