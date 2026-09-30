using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.LookupFgsTenantServiceAccountsSetups;

public sealed class LookupFgsTenantServiceAccountsSetupsQueryHandler(
    IFgsTenantServiceAccountsSetupReadRepository readRepository)
    : IRequestHandler<LookupFgsTenantServiceAccountsSetupsQuery, ApiResponse<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>>> Handle(
        LookupFgsTenantServiceAccountsSetupsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>>.Ok(result);
    }
}
