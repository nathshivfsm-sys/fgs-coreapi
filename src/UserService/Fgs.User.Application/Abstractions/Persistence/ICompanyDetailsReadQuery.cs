using Fgs.User.Application.Features.Companies.Dtos;

namespace Fgs.User.Application.Abstractions.Persistence;

public interface ICompanyDetailsReadQuery
{
    Task<CompanyDetailDto?> GetAsync(
        long tenantId,
        long companyNumber,
        CancellationToken cancellationToken = default);
}
