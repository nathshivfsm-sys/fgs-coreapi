using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.LookupJobTypeSubCategories;

public sealed class LookupJobTypeSubCategoriesQueryHandler(IJobTypeSubCategoryReadRepository readRepository)
    : IRequestHandler<LookupJobTypeSubCategoriesQuery, ApiResponse<IReadOnlyList<JobTypeSubCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<JobTypeSubCategoryLookupDto>>> Handle(
        LookupJobTypeSubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<JobTypeSubCategoryLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<JobTypeSubCategoryLookupDto>>(ex);
        }
    }
}
