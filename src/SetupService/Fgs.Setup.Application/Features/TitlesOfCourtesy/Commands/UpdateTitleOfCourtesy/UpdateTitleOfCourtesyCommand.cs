using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.UpdateTitleOfCourtesy;

public sealed record UpdateTitleOfCourtesyCommand(long Id, TitleOfCourtesyUpdateDto Dto)
    : IRequest<ApiResponse<TitleOfCourtesyDetailDto>>;
