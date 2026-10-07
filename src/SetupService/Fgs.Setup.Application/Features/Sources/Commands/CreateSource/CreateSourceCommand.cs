using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Commands.CreateSource;

public sealed record CreateSourceCommand(SourceCreateDto Dto)
    : IRequest<ApiResponse<SourceDetailDto>>;
