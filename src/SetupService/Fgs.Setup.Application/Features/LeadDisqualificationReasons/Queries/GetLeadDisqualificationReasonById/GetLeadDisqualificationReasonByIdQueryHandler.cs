using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.GetLeadDisqualificationReasonById;

public sealed class GetLeadDisqualificationReasonByIdQueryHandler(ILeadDisqualificationReasonReadRepository readRepository)
    : IRequestHandler<GetLeadDisqualificationReasonByIdQuery, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        GetLeadDisqualificationReasonByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<LeadDisqualificationReasonDetailDto>.Fail(
                    [$"Lead Disqualification Reason '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<LeadDisqualificationReasonDetailDto>(ex);
        }
    }
}
