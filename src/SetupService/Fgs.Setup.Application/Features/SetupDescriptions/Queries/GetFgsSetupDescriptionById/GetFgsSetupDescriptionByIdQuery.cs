using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.GetFgsSetupDescriptionById;

public sealed record GetFgsSetupDescriptionByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupDescriptionDetailDto>>;
