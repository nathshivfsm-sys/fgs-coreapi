using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.CreateFgsBusinessType;

public sealed class CreateFgsBusinessTypeCommandHandler(
    IFgsBusinessTypeWriteService writeService,
    ILogger<CreateFgsBusinessTypeCommandHandler> logger)
    : IRequestHandler<CreateFgsBusinessTypeCommand, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        CreateFgsBusinessTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.CreateAsync(request.Dto, cancellationToken);
            logger.LogInformation("Created business type {Id} with code {Code}", result.Id, result.Code);
            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create business type");
            return CatalogCrudExceptionMapper.MapException<FgsBusinessTypeDetailDto>(ex);
        }
    }
}
