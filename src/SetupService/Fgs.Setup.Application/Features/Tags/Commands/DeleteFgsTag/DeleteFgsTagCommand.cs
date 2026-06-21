using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Commands.DeleteFgsTag;

public sealed record DeleteFgsTagCommand(long Id)
    : IRequest<ApiResponse<FgsTagDetailDto>>;
