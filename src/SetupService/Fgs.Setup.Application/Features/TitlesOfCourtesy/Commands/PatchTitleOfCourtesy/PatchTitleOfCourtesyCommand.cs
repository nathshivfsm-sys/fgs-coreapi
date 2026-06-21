using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.PatchTitleOfCourtesy;

public sealed record PatchTitleOfCourtesyCommand(long Id, TitleOfCourtesyPatchDto Dto)
    : IRequest<ApiResponse<TitleOfCourtesyDetailDto>>;
