using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Bff.Application.Features.Lookups.Queries.ListLookupKeys;

public sealed class ListLookupKeysQueryHandler
    : IRequestHandler<ListLookupKeysQuery, ApiResponse<IReadOnlyList<LookupKeyInfoDto>>>
{
    public Task<ApiResponse<IReadOnlyList<LookupKeyInfoDto>>> Handle(
        ListLookupKeysQuery request,
        CancellationToken cancellationToken)
    {
        var items = LookupCatalog.All
            .Select(d => new LookupKeyInfoDto(
                d.Key,
                d.Service,
                d.RelativePath,
                d.RequiresTenant,
                d.RequiredFilters,
                d.OptionalFilters,
                d.Description))
            .ToList();

        return Task.FromResult(ApiResponse<IReadOnlyList<LookupKeyInfoDto>>.Ok(items));
    }
}
