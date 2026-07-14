using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Commands.CreateFgsDataAccess;

public sealed record CreateFgsDataAccessCommand(FgsDataAccessCreateDto Dto)
    : IRequest<ApiResponse<FgsDataAccessDetailDto>>;
