using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.LeadSources.Commands.DeleteLeadSource;

public sealed class DeleteLeadSourceCommandHandler(
    ILeadSourceWriteService writeService,
    ILogger<DeleteLeadSourceCommandHandler> logger)
    : IRequestHandler<DeleteLeadSourceCommand, ApiResponse<LeadSourceDetailDto>>
{
    public async Task<ApiResponse<LeadSourceDetailDto>> Handle(
        DeleteLeadSourceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted lead source {Id}", result.Id);
            return ApiResponse<LeadSourceDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete lead source {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<LeadSourceDetailDto>(ex);
        }
    }
}
