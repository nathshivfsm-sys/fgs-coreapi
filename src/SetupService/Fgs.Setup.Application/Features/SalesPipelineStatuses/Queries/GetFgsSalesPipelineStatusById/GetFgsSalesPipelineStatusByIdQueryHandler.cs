using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.GetFgsSalesPipelineStatusById;

public sealed class GetFgsSalesPipelineStatusByIdQueryHandler(IFgsSalesPipelineStatusReadRepository readRepository)
    : IRequestHandler<GetFgsSalesPipelineStatusByIdQuery, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        GetFgsSalesPipelineStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsSalesPipelineStatusDetailDto>.Fail(
                    [$"Sales Pipeline Status '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSalesPipelineStatusDetailDto>(ex);
        }
    }
}
