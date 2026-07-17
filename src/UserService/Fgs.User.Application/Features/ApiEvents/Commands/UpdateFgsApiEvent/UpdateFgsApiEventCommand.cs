using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Commands.UpdateFgsApiEvent;

public sealed record UpdateFgsApiEventCommand(long Id, FgsApiEventUpdateDto Dto)
    : IRequest<ApiResponse<FgsApiEventDetailDto>>;
