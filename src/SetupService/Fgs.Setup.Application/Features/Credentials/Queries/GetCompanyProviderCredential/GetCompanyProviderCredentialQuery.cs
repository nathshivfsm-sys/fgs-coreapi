using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.GetCompanyProviderCredential;

public sealed record GetCompanyProviderCredentialQuery(string ProviderCode)
    : IRequest<ApiResponse<CompanyProviderCredentialDto>>;
