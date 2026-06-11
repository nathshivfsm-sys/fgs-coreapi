using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud.Abstractions;
using MediatR;

namespace Fgs.Foundation.CatalogCrud.Commands;

public sealed record CreateCatalogEntityCommand<TCreate, TDetail>(string EntityKey, TCreate Payload)
    : IRequest<ApiResponse<TDetail>>;

public sealed class CreateCatalogEntityCommandHandler<TCreate, TDetail>
    : IRequestHandler<CreateCatalogEntityCommand<TCreate, TDetail>, ApiResponse<TDetail>>
{
    private readonly IEntityRegistry _entityRegistry;
    private readonly IEntityWriteService _writeService;

    public CreateCatalogEntityCommandHandler(IEntityRegistry entityRegistry, IEntityWriteService writeService)
    {
        _entityRegistry = entityRegistry;
        _writeService = writeService;
    }

    public async Task<ApiResponse<TDetail>> Handle(
        CreateCatalogEntityCommand<TCreate, TDetail> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var descriptor = _entityRegistry.GetRequired(request.EntityKey);
            if (descriptor.CreateDtoType != typeof(TCreate) || descriptor.DetailDtoType != typeof(TDetail))
            {
                return ApiResponse<TDetail>.Fail(["DTO type mismatch."], ApiStatusCodes.BadRequest);
            }

            var result = await _writeService.CreateAsync(descriptor, request.Payload!, cancellationToken);
            return ApiResponse<TDetail>.Ok((TDetail)result, ApiStatusCodes.Created);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<TDetail>(ex);
        }
    }
}
