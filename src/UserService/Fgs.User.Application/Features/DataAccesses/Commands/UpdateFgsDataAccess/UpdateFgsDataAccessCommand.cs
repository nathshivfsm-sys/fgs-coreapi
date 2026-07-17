using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Commands.UpdateFgsDataAccess;

public sealed record UpdateFgsDataAccessCommand(long Id, FgsDataAccessUpdateDto Dto)
    : IRequest<ApiResponse<FgsDataAccessDetailDto>>;
