using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Queries.GetFgsPermissionById;

public sealed record GetFgsPermissionByIdQuery(long Id) : IRequest<ApiResponse<FgsPermissionDetailDto>>;
