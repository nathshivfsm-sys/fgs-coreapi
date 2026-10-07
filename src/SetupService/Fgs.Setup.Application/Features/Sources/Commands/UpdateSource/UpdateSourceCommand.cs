using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Commands.UpdateSource;

public sealed record UpdateSourceCommand(long Id, SourceUpdateDto Dto)
    : IRequest<ApiResponse<SourceDetailDto>>;
