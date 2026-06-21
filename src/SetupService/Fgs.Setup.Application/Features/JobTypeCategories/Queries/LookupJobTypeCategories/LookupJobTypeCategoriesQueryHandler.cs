using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.LookupJobTypeCategories;

public sealed class LookupJobTypeCategoriesQueryHandler(IJobTypeCategoryReadRepository readRepository)
    : IRequestHandler<LookupJobTypeCategoriesQuery, ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>> Handle(
        LookupJobTypeCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<JobTypeCategoryLookupDto>>(ex);
        }
    }
}
