using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud.Abstractions;
using MediatR;

namespace Fgs.Foundation.CatalogCrud.Commands;

public sealed record UpdateCatalogEntityCommand<TUpdate, TDetail>(
    string EntityKey,
    string Id,
    TUpdate Payload) : IRequest<ApiResponse<TDetail>>;

public sealed class UpdateCatalogEntityCommandHandler<TUpdate, TDetail>
    : IRequestHandler<UpdateCatalogEntityCommand<TUpdate, TDetail>, ApiResponse<TDetail>>
{
    private readonly IEntityRegistry _entityRegistry;
    private readonly IEntityWriteService _writeService;

    public UpdateCatalogEntityCommandHandler(IEntityRegistry entityRegistry, IEntityWriteService writeService)
    {
        _entityRegistry = entityRegistry;
        _writeService = writeService;
    }

    public async Task<ApiResponse<TDetail>> Handle(
        UpdateCatalogEntityCommand<TUpdate, TDetail> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var descriptor = _entityRegistry.GetRequired(request.EntityKey);
            if (descriptor.UpdateDtoType != typeof(TUpdate) || descriptor.DetailDtoType != typeof(TDetail))
            {
                return ApiResponse<TDetail>.Fail(["DTO type mismatch."], ApiStatusCodes.BadRequest);
            }

            var result = await _writeService.UpdateAsync(
                descriptor,
                request.Id,
                request.Payload!,
                cancellationToken);

            return ApiResponse<TDetail>.Ok((TDetail)result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<TDetail>(ex);
        }
    }
}
