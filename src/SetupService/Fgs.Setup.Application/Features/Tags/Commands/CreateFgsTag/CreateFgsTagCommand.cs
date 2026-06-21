using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Commands.CreateFgsTag;

public sealed record CreateFgsTagCommand(FgsTagCreateDto Dto)
    : IRequest<ApiResponse<FgsTagDetailDto>>;
