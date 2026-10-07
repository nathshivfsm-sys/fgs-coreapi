using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Commands.PatchSource;

public sealed record PatchSourceCommand(long Id, SourcePatchDto Dto)
    : IRequest<ApiResponse<SourceDetailDto>>;
