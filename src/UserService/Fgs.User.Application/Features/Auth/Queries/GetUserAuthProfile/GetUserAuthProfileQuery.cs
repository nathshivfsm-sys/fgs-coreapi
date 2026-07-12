using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.GetUserAuthProfile;

public sealed record GetUserAuthProfileQuery(string EntraObjectId)
    : IRequest<ApiResponse<UserAuthProfileResultDto>>;

public sealed record UserAuthProfileResultDto(
    Guid UserId,
    string Email,
    string? EntraObjectId,
    long TenantId,
    long CompanyId,
    bool IsActive,
    bool IsDeleted,
    IReadOnlyList<string> Roles);
