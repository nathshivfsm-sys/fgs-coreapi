using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.UpdateFgsBusinessType;

public sealed class UpdateFgsBusinessTypeCommandHandler(
    IFgsBusinessTypeWriteService writeService,
    ILogger<UpdateFgsBusinessTypeCommandHandler> logger)
    : IRequestHandler<UpdateFgsBusinessTypeCommand, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        UpdateFgsBusinessTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated business type {Id}", result.Id);
            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update business type {Id}", request.Id);
            return CatalogCrudExceptionMapper.MapException<FgsBusinessTypeDetailDto>(ex);
        }
    }
}
