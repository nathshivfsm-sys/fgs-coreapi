using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.DeleteFgsBusinessType;

public sealed class DeleteFgsBusinessTypeCommandHandler(
    IFgsBusinessTypeWriteService writeService,
    ILogger<DeleteFgsBusinessTypeCommandHandler> logger)
    : IRequestHandler<DeleteFgsBusinessTypeCommand, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        DeleteFgsBusinessTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted business type {Id}", result.Id);
            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete business type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsBusinessTypeDetailDto>(ex);
        }
    }
}
