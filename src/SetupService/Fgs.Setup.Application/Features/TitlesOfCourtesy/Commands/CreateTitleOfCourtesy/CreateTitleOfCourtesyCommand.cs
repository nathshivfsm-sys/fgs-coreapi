using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;

public sealed record CreateTitleOfCourtesyCommand(TitleOfCourtesyCreateDto Dto)
    : IRequest<ApiResponse<TitleOfCourtesyDetailDto>>;
