using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud.Abstractions;
using MediatR;

namespace Fgs.Foundation.CatalogCrud.Queries;

public sealed record ListCatalogEntitiesQuery<TSummary>(
    string EntityKey,
    PagedQuery Paging,
    object? Filters = null) : IRequest<ApiResponse<PagedResult<TSummary>>>;

public sealed class ListCatalogEntitiesQueryHandler<TSummary>
    : IRequestHandler<ListCatalogEntitiesQuery<TSummary>, ApiResponse<PagedResult<TSummary>>>
{
    private readonly IEntityRegistry _entityRegistry;
    private readonly IEntityReadRepository _readRepository;

    public ListCatalogEntitiesQueryHandler(IEntityRegistry entityRegistry, IEntityReadRepository readRepository)
    {
        _entityRegistry = entityRegistry;
        _readRepository = readRepository;
    }

    public async Task<ApiResponse<PagedResult<TSummary>>> Handle(
        ListCatalogEntitiesQuery<TSummary> request,
        CancellationToken cancellationToken)
    {
        var descriptor = _entityRegistry.GetRequired(request.EntityKey);
        if (descriptor.SummaryDtoType != typeof(TSummary))
        {
            return ApiResponse<PagedResult<TSummary>>.Fail(["DTO type mismatch."], ApiStatusCodes.BadRequest);
        }

        var filters = CatalogEntityMapper.ExtractFilters(request.Filters)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString(), StringComparer.OrdinalIgnoreCase);

        var result = await _readRepository.ListAsync(
            descriptor,
            request.Paging,
            filters,
            typeof(TSummary),
            cancellationToken);

        var typedItems = result.Items.Cast<TSummary>().ToList();
        return ApiResponse<PagedResult<TSummary>>.Ok(
            new PagedResult<TSummary>(typedItems, result.Page, result.PageSize, result.TotalCount));
    }
}
