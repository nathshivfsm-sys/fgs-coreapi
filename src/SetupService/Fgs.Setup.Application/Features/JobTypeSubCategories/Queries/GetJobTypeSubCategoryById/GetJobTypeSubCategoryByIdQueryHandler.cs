using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.GetJobTypeSubCategoryById;

public sealed class GetJobTypeSubCategoryByIdQueryHandler(IJobTypeSubCategoryReadRepository readRepository)
    : IRequestHandler<GetJobTypeSubCategoryByIdQuery, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        GetJobTypeSubCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<JobTypeSubCategoryDetailDto>.Fail(
                    [$"Job Type Subcategory '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<JobTypeSubCategoryDetailDto>(ex);
        }
    }
}
