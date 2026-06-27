using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.DeleteTitleOfCourtesy;

public sealed record DeleteTitleOfCourtesyCommand(long Id)
    : IRequest<ApiResponse<TitleOfCourtesyDetailDto>>;
