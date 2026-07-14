using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Commands.CreateFgsApiEvent;

public sealed record CreateFgsApiEventCommand(FgsApiEventCreateDto Dto)
    : IRequest<ApiResponse<FgsApiEventDetailDto>>;
