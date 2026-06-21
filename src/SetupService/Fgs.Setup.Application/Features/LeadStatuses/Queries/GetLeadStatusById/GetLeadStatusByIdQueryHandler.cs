using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.GetLeadStatusById;

public sealed class GetLeadStatusByIdQueryHandler(ILeadStatusReadRepository readRepository)
    : IRequestHandler<GetLeadStatusByIdQuery, ApiResponse<LeadStatusDetailDto>>
{
    public async Task<ApiResponse<LeadStatusDetailDto>> Handle(
        GetLeadStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<LeadStatusDetailDto>.Fail(
                    [$"Lead Status '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<LeadStatusDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<LeadStatusDetailDto>(ex);
        }
    }
}
