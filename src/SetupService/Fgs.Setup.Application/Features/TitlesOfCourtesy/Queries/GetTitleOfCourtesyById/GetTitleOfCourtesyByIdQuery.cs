using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.GetTitleOfCourtesyById;

public sealed record GetTitleOfCourtesyByIdQuery(long Id)
    : IRequest<ApiResponse<TitleOfCourtesyDetailDto>>;
