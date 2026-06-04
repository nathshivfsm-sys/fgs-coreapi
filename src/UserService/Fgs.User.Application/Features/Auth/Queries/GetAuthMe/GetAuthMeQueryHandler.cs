using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Security.Abstractions;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.GetAuthMe;

public sealed class GetAuthMeQueryHandler(IFgsUserContext userContext)
    : IRequestHandler<GetAuthMeQuery, ApiResponse<FgsAuthMeDto>>
{
    public Task<ApiResponse<FgsAuthMeDto>> Handle(GetAuthMeQuery request, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated
            || userContext.UserId is null
            || userContext.EntraObjectId is null
            || userContext.TenantId is null
            || userContext.CompanyId is null
            || string.IsNullOrWhiteSpace(userContext.Email))
        {
            return Task.FromResult(ApiResponse<FgsAuthMeDto>.Fail(
                ["Unauthorized."],
                ApiStatusCodes.Unauthorized));
        }

        return Task.FromResult(ApiResponse<FgsAuthMeDto>.Ok(new FgsAuthMeDto(
            userContext.UserId.Value,
            userContext.Email,
            userContext.EntraObjectId,
            userContext.TenantId.Value,
            userContext.CompanyId.Value,
            userContext.Roles)));
    }
}
