using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Commands.PatchFgsTag;

public sealed record PatchFgsTagCommand(long Id, FgsTagPatchDto Dto)
    : IRequest<ApiResponse<FgsTagDetailDto>>;
