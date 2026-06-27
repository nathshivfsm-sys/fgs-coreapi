using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Commands.UpdateFgsTag;

public sealed record UpdateFgsTagCommand(long Id, FgsTagUpdateDto Dto)
    : IRequest<ApiResponse<FgsTagDetailDto>>;
