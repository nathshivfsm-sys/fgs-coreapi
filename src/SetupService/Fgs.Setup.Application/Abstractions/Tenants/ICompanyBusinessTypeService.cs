using Fgs.Contracts.Clients;

namespace Fgs.Setup.Application.Abstractions.Tenants;

public interface ICompanyBusinessTypeService
{
    Task AddCompanyBusinessTypesAsync(
        long tenantId,
        long companyId,
        AddCompanyBusinessTypesRequest request,
        CancellationToken cancellationToken = default);
}
