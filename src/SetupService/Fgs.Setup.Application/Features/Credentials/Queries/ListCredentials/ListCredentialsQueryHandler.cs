using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.ListCredentials;

public sealed class ListCredentialsQueryHandler(
    ICredentialRepository repository,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListCredentialsQuery, ApiResponse<IReadOnlyList<CredentialSummaryDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<CredentialSummaryDto>>> Handle(
        ListCredentialsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Scope != CredentialScope.Global && request.Scope != CredentialScope.Tenant)
        {
            return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Fail(
                [CredentialErrorMessages.InvalidScope],
                ApiStatusCodes.BadRequest);
        }

        if (request.Scope == CredentialScope.Global)
        {
            var globalItems = (await repository.ListGlobalAsync(request.ActiveOnly, cancellationToken))
                .Select(CredentialMapper.ToSummary)
                .ToList();
            return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Ok(globalItems);
        }

        if (tenantContextAccessor.Current is not { } tenantScope)
        {
            return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Fail(
                [CredentialErrorMessages.TenantContextRequired],
                ApiStatusCodes.BadRequest);
        }

        if (request.TenantId is { } requestedTenant && requestedTenant != tenantScope.TenantId)
        {
            return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Fail(
                [CredentialErrorMessages.TenantScopeMismatch],
                ApiStatusCodes.Forbidden);
        }

        if (request.CompanyId is { } requestedCompany && requestedCompany != tenantScope.CompanyId)
        {
            return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Fail(
                [CredentialErrorMessages.TenantScopeMismatch],
                ApiStatusCodes.Forbidden);
        }

        var tenantItems = (await repository.ListTenantAsync(
                tenantScope.TenantId,
                tenantScope.CompanyId,
                request.ActiveOnly,
                cancellationToken))
            .Select(CredentialMapper.ToSummary)
            .ToList();

        return ApiResponse<IReadOnlyList<CredentialSummaryDto>>.Ok(tenantItems);
    }
}
