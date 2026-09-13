using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.GetCredential;

public sealed class GetCredentialQueryHandler(
    ICredentialRepository repository,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetCredentialQuery, ApiResponse<CredentialDetailDto>>
{
    public async Task<ApiResponse<CredentialDetailDto>> Handle(
        GetCredentialQuery request,
        CancellationToken cancellationToken)
    {
        return request.Scope switch
        {
            CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId) =>
                await GetGlobalAsync(globalId, cancellationToken),
            CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantCredentialId) =>
                await GetTenantAsync(tenantCredentialId, cancellationToken),
            _ => ApiResponse<CredentialDetailDto>.Fail([CredentialErrorMessages.InvalidScope], ApiStatusCodes.BadRequest)
        };
    }

    private async Task<ApiResponse<CredentialDetailDto>> GetGlobalAsync(int id, CancellationToken cancellationToken)
    {
        var credential = await repository.GetGlobalByIdAsync(id, cancellationToken);
        return credential is null
            ? ApiResponse<CredentialDetailDto>.Fail([CredentialErrorMessages.GlobalCredentialNotFound], ApiStatusCodes.NotFound)
            : ApiResponse<CredentialDetailDto>.Ok(CredentialMapper.ToDetail(credential));
    }

    private async Task<ApiResponse<CredentialDetailDto>> GetTenantAsync(Guid id, CancellationToken cancellationToken)
    {
        var credential = await repository.GetTenantByIdAsync(id, cancellationToken);
        if (credential is null)
        {
            return ApiResponse<CredentialDetailDto>.Fail(
                [CredentialErrorMessages.TenantCredentialNotFound],
                ApiStatusCodes.NotFound);
        }

        var scopeDenied = CredentialRequestHelpers.EnsureTenantScope<CredentialDetailDto>(
            tenantContextAccessor,
            credential);
        return scopeDenied ?? ApiResponse<CredentialDetailDto>.Ok(CredentialMapper.ToDetail(credential));
    }
}
