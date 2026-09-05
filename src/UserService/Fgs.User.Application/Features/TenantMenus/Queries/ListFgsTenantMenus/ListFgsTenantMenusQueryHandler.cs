using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Queries.ListFgsTenantMenus;

public sealed class ListFgsTenantMenusQueryHandler(IFgsTenantMenuReadRepository readRepository)
    : IRequestHandler<ListFgsTenantMenusQuery, ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>> Handle(
        ListFgsTenantMenusQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>.Ok(result);
    }
}
