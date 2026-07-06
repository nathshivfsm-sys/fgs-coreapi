using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetFgsSetupCommunicationTemplateById;

public sealed class GetFgsSetupCommunicationTemplateByIdQueryHandler(
    IFgsSetupCommunicationTemplateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSetupCommunicationTemplateByIdQuery, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        GetFgsSetupCommunicationTemplateByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "communication-template",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSetupCommunicationTemplateDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Fail(
                [$"Communication Template '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result);
    }
}
