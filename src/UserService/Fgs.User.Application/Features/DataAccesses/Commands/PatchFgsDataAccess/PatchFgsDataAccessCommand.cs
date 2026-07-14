using Fgs.Contracts.Api;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Commands.PatchFgsDataAccess;

public sealed record PatchFgsDataAccessCommand(long Id, FgsDataAccessPatchDto Dto)
    : IRequest<ApiResponse<FgsDataAccessDetailDto>>;
