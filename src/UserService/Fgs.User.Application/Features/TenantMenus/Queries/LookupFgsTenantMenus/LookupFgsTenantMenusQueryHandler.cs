using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Queries.LookupFgsTenantMenus;

public sealed class LookupFgsTenantMenusQueryHandler(IFgsTenantMenuReadRepository readRepository)
    : IRequestHandler<LookupFgsTenantMenusQuery, ApiResponse<IReadOnlyList<FgsTenantMenuLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTenantMenuLookupDto>>> Handle(
        LookupFgsTenantMenusQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsTenantMenuLookupDto>>.Ok(result);
    }
}
