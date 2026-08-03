using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetup;

public sealed class GetFgsTenantServiceAccountsSetupQueryHandler(
    IFgsTenantServiceAccountsSetupReadRepository readRepository)
    : IRequestHandler<GetFgsTenantServiceAccountsSetupQuery, ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>> Handle(
        GetFgsTenantServiceAccountsSetupQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetCurrentAsync(cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTenantServiceAccountsSetupDetailDto>.Fail(
                ["Service accounts setup was not found for the current company."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsTenantServiceAccountsSetupDetailDto>.Ok(result);
    }
}
