using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.ListActiveCommunicationTemplates;

public sealed class ListActiveCommunicationTemplatesQueryHandler(
    IFgsSetupCommunicationTemplateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveCommunicationTemplatesQuery, ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>> Handle(
        ListActiveCommunicationTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(
        request.Page,
        request.PageSize,
        request.SortBy,
        request.SortDirection.ToString(),
        request.Search,
        CacheKeys.Fingerprint(request.Filters));

        var cacheKey = CacheKeys.Build(
        tenantScope.TenantId,
        tenantScope.CompanyId,
        "communicationtemplate",
        segment);

        var cached = await cache.GetOrSetAsync(
        cacheKey,
        async () =>
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            return await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupCommunicationTemplateListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSetupCommunicationTemplateSummaryDto>>.Ok(cached!);
    }
}
