using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadSources.Queries.GetLeadSourceById;

public sealed class GetLeadSourceByIdQueryHandler(ILeadSourceReadRepository readRepository)
    : IRequestHandler<GetLeadSourceByIdQuery, ApiResponse<LeadSourceDetailDto>>
{
    public async Task<ApiResponse<LeadSourceDetailDto>> Handle(
        GetLeadSourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<LeadSourceDetailDto>.Fail(
                    [$"Lead Source '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<LeadSourceDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<LeadSourceDetailDto>(ex);
        }
    }
}
