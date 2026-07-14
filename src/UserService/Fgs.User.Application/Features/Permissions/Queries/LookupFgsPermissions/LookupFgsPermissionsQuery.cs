using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Queries.LookupFgsPermissions;

public sealed record LookupFgsPermissionsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsPermissionLookupDto>>>;
