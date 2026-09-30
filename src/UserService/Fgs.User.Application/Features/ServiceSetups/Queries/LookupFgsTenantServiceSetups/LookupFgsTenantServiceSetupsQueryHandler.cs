using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.LookupFgsTenantServiceSetups;

public sealed class LookupFgsTenantServiceSetupsQueryHandler(IFgsTenantServiceSetupReadRepository readRepository)
    : IRequestHandler<LookupFgsTenantServiceSetupsQuery, ApiResponse<IReadOnlyList<FgsTenantServiceSetupLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTenantServiceSetupLookupDto>>> Handle(
        LookupFgsTenantServiceSetupsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsTenantServiceSetupLookupDto>>.Ok(result);
    }
}
