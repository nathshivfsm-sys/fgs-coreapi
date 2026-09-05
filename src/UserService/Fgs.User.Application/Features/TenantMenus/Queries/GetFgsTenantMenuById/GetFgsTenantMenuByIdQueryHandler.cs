using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.TenantMenus.Queries.GetFgsTenantMenuById;

public sealed class GetFgsTenantMenuByIdQueryHandler(IFgsTenantMenuReadRepository readRepository)
    : IRequestHandler<GetFgsTenantMenuByIdQuery, ApiResponse<FgsTenantMenuDetailDto>>
{
    public async Task<ApiResponse<FgsTenantMenuDetailDto>> Handle(
        GetFgsTenantMenuByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTenantMenuDetailDto>.Fail(
                [$"Tenant menu '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsTenantMenuDetailDto>.Ok(result);
    }
}
