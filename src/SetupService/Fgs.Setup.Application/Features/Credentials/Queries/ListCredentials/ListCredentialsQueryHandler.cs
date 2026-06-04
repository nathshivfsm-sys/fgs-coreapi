using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.ListCredentials;

public sealed class ListCredentialsQueryHandler
    : IRequestHandler<ListCredentialsQuery, ApiResponse<IReadOnlyList<CredentialSummaryDto>>>
{
    private readonly ICredentialRepository _repository;

    public ListCredentialsQueryHandler(ICredentialRepository repository) => _repository = repository;

    public async Task<ApiResponse<IReadOnlyList<CredentialSummaryDto>>> Handle(
        ListCredentialsQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<CredentialSummaryDto> items = request.Scope switch
        {
            CredentialScope.Global => (await _repository.ListGlobalAsync(request.ActiveOnly, cancellationToken))
                .Select(CredentialMapper.ToSummary)
                .ToList(),
            CredentialScope.Tenant => (await _repository.ListTenantAsync(
                    request.TenantId,
                    request.CompanyId,
                    request.ActiveOnly,
                    cancellationToken))
                .Select(CredentialMapper.ToSummary)
                .ToList(),
            _ => []
        };

        if (request.Scope != CredentialScope.Global && request.Scope != CredentialScope.Tenant)
        {
            return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Fail(
                [CredentialErrorMessages.InvalidScope],
                ApiStatusCodes.BadRequest);
        }

        return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Ok(items);
    }
}

