using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Commands.PatchFgsApiEvent;

public sealed record PatchFgsApiEventCommand(long Id, FgsApiEventPatchDto Dto)
    : IRequest<ApiResponse<FgsApiEventDetailDto>>;
