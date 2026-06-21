using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.LookupJobTypes;

public sealed class LookupJobTypesQueryHandler(IJobTypeReadRepository readRepository)
    : IRequestHandler<LookupJobTypesQuery, ApiResponse<IReadOnlyList<JobTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<JobTypeLookupDto>>> Handle(
        LookupJobTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<JobTypeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<JobTypeLookupDto>>(ex);
        }
    }
}
