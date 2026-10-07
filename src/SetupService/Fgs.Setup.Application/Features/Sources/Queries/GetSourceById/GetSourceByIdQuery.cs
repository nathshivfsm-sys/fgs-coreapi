using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Queries.GetSourceById;

public sealed record GetSourceByIdQuery(long Id)
    : IRequest<ApiResponse<SourceDetailDto>>;
