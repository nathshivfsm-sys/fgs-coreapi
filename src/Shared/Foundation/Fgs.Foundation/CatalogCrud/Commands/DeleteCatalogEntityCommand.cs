using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud.Abstractions;
using MediatR;

namespace Fgs.Foundation.CatalogCrud.Commands;

public sealed record DeleteCatalogEntityCommand(string EntityKey, string Id)
    : IRequest<ApiResponse<object>>;

public sealed class DeleteCatalogEntityCommandHandler
    : IRequestHandler<DeleteCatalogEntityCommand, ApiResponse<object>>
{
    private readonly IEntityRegistry _entityRegistry;
    private readonly IEntityWriteService _writeService;

    public DeleteCatalogEntityCommandHandler(IEntityRegistry entityRegistry, IEntityWriteService writeService)
    {
        _entityRegistry = entityRegistry;
        _writeService = writeService;
    }

    public async Task<ApiResponse<object>> Handle(
        DeleteCatalogEntityCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var descriptor = _entityRegistry.GetRequired(request.EntityKey);
            await _writeService.DeleteAsync(descriptor, request.Id, cancellationToken);
            return ApiResponse<object>.Ok(new object());
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<object>(ex);
        }
    }
}
