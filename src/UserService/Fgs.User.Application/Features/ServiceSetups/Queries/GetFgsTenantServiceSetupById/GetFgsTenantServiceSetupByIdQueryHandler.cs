using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetupById;

public sealed class GetFgsTenantServiceSetupByIdQueryHandler(IFgsTenantServiceSetupReadRepository readRepository)
    : IRequestHandler<GetFgsTenantServiceSetupByIdQuery, ApiResponse<FgsTenantServiceSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceSetupDetailDto>> Handle(
        GetFgsTenantServiceSetupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetCurrentAsync(cancellationToken);
        if (result is null || result.CompanyId != request.CompanyId)
        {
            return ApiResponse<FgsTenantServiceSetupDetailDto>.Fail(
                [$"Service setup for company '{request.CompanyId}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsTenantServiceSetupDetailDto>.Ok(result);
    }
}
