using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.DeleteTitleOfCourtesy;

public sealed class DeleteTitleOfCourtesyCommandHandler(
    ITitleOfCourtesyWriteService writeService,
    ILogger<DeleteTitleOfCourtesyCommandHandler> logger)
    : IRequestHandler<DeleteTitleOfCourtesyCommand, ApiResponse<TitleOfCourtesyDetailDto>>
{
    public async Task<ApiResponse<TitleOfCourtesyDetailDto>> Handle(
        DeleteTitleOfCourtesyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation(
                "Soft-deleted title of courtesy {TitleOfCourtesyId} with code {Code}",
                result.Id,
                result.Code);

        return ApiResponse<TitleOfCourtesyDetailDto>.Ok(result);
    }
}
