using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.UpdateTitleOfCourtesy;

public sealed class UpdateTitleOfCourtesyCommandHandler(
    ITitleOfCourtesyWriteService writeService,
    ILogger<UpdateTitleOfCourtesyCommandHandler> logger)
    : IRequestHandler<UpdateTitleOfCourtesyCommand, ApiResponse<TitleOfCourtesyDetailDto>>
{
    public async Task<ApiResponse<TitleOfCourtesyDetailDto>> Handle(
        UpdateTitleOfCourtesyCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation(
                "Updated title of courtesy {TitleOfCourtesyId} with code {Code}",
                result.Id,
                result.Code);

            return ApiResponse<TitleOfCourtesyDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update title of courtesy {TitleOfCourtesyId}", request.Id);
            return CatalogCrudExceptionMapper.MapException<TitleOfCourtesyDetailDto>(ex);
        }
    }
}
