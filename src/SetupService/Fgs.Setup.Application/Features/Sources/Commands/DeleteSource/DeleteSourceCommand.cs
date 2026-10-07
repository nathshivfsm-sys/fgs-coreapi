using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Commands.DeleteSource;

public sealed record DeleteSourceCommand(long Id)
    : IRequest<ApiResponse<SourceDetailDto>>;
