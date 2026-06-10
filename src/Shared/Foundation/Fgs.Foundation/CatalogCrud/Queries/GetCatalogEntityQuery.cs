using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud.Abstractions;
using MediatR;

namespace Fgs.Foundation.CatalogCrud.Queries;

public sealed record GetCatalogEntityQuery<TDetail>(string EntityKey, string Id)
    : IRequest<ApiResponse<TDetail>>;

public sealed class GetCatalogEntityQueryHandler<TDetail>
    : IRequestHandler<GetCatalogEntityQuery<TDetail>, ApiResponse<TDetail>>
{
    private readonly IEntityRegistry _entityRegistry;
    private readonly IEntityReadRepository _readRepository;

    public GetCatalogEntityQueryHandler(IEntityRegistry entityRegistry, IEntityReadRepository readRepository)
    {
        _entityRegistry = entityRegistry;
        _readRepository = readRepository;
    }

    public async Task<ApiResponse<TDetail>> Handle(
        GetCatalogEntityQuery<TDetail> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var descriptor = _entityRegistry.GetRequired(request.EntityKey);
            if (descriptor.DetailDtoType != typeof(TDetail))
            {
                return ApiResponse<TDetail>.Fail(["DTO type mismatch."], ApiStatusCodes.BadRequest);
            }

            var result = await _readRepository.GetByIdAsync(descriptor, request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<TDetail>.Fail(
                    [$"{descriptor.EntityName} '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<TDetail>.Ok((TDetail)result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<TDetail>(ex);
        }
    }
}
