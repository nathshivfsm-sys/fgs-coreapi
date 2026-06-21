using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.GetJobTypeById;

public sealed class GetJobTypeByIdQueryHandler(IJobTypeReadRepository readRepository)
    : IRequestHandler<GetJobTypeByIdQuery, ApiResponse<JobTypeDetailDto>>
{
    public async Task<ApiResponse<JobTypeDetailDto>> Handle(
        GetJobTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<JobTypeDetailDto>.Fail(
                    [$"Job Type '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<JobTypeDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<JobTypeDetailDto>(ex);
        }
    }
}
