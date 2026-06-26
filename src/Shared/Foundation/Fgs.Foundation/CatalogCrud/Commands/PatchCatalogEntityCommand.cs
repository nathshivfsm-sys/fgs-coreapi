using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud.Abstractions;
using MediatR;

namespace Fgs.Foundation.CatalogCrud.Commands;

public sealed record PatchCatalogEntityCommand<TPatch, TDetail>(
    string EntityKey,
    string Id,
    TPatch Payload) : IRequest<ApiResponse<TDetail>>;

public sealed class PatchCatalogEntityCommandHandler<TPatch, TDetail>
    : IRequestHandler<PatchCatalogEntityCommand<TPatch, TDetail>, ApiResponse<TDetail>>
{
    private readonly IEntityRegistry _entityRegistry;
    private readonly IEntityWriteService _writeService;

    public PatchCatalogEntityCommandHandler(IEntityRegistry entityRegistry, IEntityWriteService writeService)
    {
        _entityRegistry = entityRegistry;
        _writeService = writeService;
    }

    public async Task<ApiResponse<TDetail>> Handle(
        PatchCatalogEntityCommand<TPatch, TDetail> request,
        CancellationToken cancellationToken)
    {
        var descriptor = _entityRegistry.GetRequired(request.EntityKey);
        if (descriptor.PatchDtoType != typeof(TPatch) || descriptor.DetailDtoType != typeof(TDetail))
        {
            return ApiResponse<TDetail>.Fail(["DTO type mismatch."], ApiStatusCodes.BadRequest);
        }

        var result = await _writeService.PatchAsync(
            descriptor,
            request.Id,
            request.Payload!,
            cancellationToken);

        return ApiResponse<TDetail>.Ok((TDetail)result);
    }
}
