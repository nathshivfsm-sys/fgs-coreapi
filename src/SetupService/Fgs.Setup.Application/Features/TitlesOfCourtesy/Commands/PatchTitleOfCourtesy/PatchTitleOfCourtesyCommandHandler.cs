using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.PatchTitleOfCourtesy;

public sealed class PatchTitleOfCourtesyCommandHandler(
    ITitleOfCourtesyWriteService writeService,
    ILogger<PatchTitleOfCourtesyCommandHandler> logger)
    : IRequestHandler<PatchTitleOfCourtesyCommand, ApiResponse<TitleOfCourtesyDetailDto>>
{
    public async Task<ApiResponse<TitleOfCourtesyDetailDto>> Handle(
        PatchTitleOfCourtesyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched title of courtesy {TitleOfCourtesyId}", result.Id);

        return ApiResponse<TitleOfCourtesyDetailDto>.Ok(result);
    }
}
