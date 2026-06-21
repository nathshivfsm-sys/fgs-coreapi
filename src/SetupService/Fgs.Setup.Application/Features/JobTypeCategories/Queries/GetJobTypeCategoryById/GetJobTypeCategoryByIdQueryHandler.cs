using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.GetJobTypeCategoryById;

public sealed class GetJobTypeCategoryByIdQueryHandler(IJobTypeCategoryReadRepository readRepository)
    : IRequestHandler<GetJobTypeCategoryByIdQuery, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        GetJobTypeCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<JobTypeCategoryDetailDto>.Fail(
                    [$"Job Type Category '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<JobTypeCategoryDetailDto>(ex);
        }
    }
}
