using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetupById;

public sealed class GetFgsTenantServiceAccountsSetupByIdQueryHandler(
    IFgsTenantServiceAccountsSetupReadRepository readRepository)
    : IRequestHandler<GetFgsTenantServiceAccountsSetupByIdQuery, ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>> Handle(
        GetFgsTenantServiceAccountsSetupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetCurrentAsync(cancellationToken);
        if (result is null || result.CompanyId != request.CompanyId)
        {
            return ApiResponse<FgsTenantServiceAccountsSetupDetailDto>.Fail(
                [$"Service accounts setup for company '{request.CompanyId}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsTenantServiceAccountsSetupDetailDto>.Ok(result);
    }
}
