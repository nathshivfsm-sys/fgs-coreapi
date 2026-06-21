using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.LookupTags;

public sealed class LookupTagsQueryHandler(IFgsTagReadRepository readRepository)
    : IRequestHandler<LookupTagsQuery, ApiResponse<IReadOnlyList<FgsTagLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTagLookupDto>>> Handle(
        LookupTagsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsTagLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsTagLookupDto>>(ex);
        }
    }
}
