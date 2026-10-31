using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.GetFgsSetupPriceBookVersionById;

public sealed record GetFgsSetupPriceBookVersionByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupPriceBookVersionDetailDto>>;
