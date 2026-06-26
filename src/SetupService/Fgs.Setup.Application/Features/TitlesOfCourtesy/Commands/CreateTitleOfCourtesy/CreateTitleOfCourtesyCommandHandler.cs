using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;

public sealed class CreateTitleOfCourtesyCommandHandler(
    ITitleOfCourtesyWriteService writeService,
    ILogger<CreateTitleOfCourtesyCommandHandler> logger)
    : IRequestHandler<CreateTitleOfCourtesyCommand, ApiResponse<TitleOfCourtesyDetailDto>>
{
    public async Task<ApiResponse<TitleOfCourtesyDetailDto>> Handle(
        CreateTitleOfCourtesyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
                "Created title of courtesy {TitleOfCourtesyId} with code {Code}",
                result.Id,
                result.Code);

        return ApiResponse<TitleOfCourtesyDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
