using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.ListEmployees;

public sealed class ListEmployeesQueryHandler(IFgsEmployeeReadRepository readRepository)
    : IRequestHandler<ListEmployeesQuery, ApiResponse<PagedResult<FgsEmployeeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsEmployeeSummaryDto>>> Handle(
        ListEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsEmployeeSummaryDto>>.Ok(result);
    }
}
