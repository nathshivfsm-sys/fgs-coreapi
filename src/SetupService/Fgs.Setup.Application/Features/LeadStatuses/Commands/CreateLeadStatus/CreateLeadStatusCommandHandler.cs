using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadStatuses.Commands.CreateLeadStatus;

public sealed class CreateLeadStatusCommandHandler(
    ILeadStatusWriteService writeService,
    ILogger<CreateLeadStatusCommandHandler> logger)
    : IRequestHandler<CreateLeadStatusCommand, ApiResponse<LeadStatusDetailDto>>
{
    public async Task<ApiResponse<LeadStatusDetailDto>> Handle(
        CreateLeadStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created lead status {Id} with code {StatusCode}", result.Id, result.StatusCode);
            return ApiResponse<LeadStatusDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create lead status");
            return CatalogCrudExceptionMapper.MapException<LeadStatusDetailDto>(ex);
        }
    }
}
