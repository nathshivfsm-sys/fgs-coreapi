using Fgs.Foundation.Result;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredential;

public sealed class GetCredentialQueryHandler : IRequestHandler<GetCredentialQuery, ApiResponse<CredentialDetailDto>>
{
    private readonly ICredentialRepository _repository;

    public GetCredentialQueryHandler(ICredentialRepository repository) => _repository = repository;

    public async Task<ApiResponse<CredentialDetailDto>> Handle(
        GetCredentialQuery request,
        CancellationToken cancellationToken)
    {
        return request.Scope switch
        {
            CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId) =>
                await GetGlobalAsync(globalId, cancellationToken),
            CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantId) =>
                await GetTenantAsync(tenantId, cancellationToken),
            _ => ApiResponse<CredentialDetailDto>.Fail([CredentialErrorMessages.InvalidScope], ApiStatusCodes.BadRequest)
        };
    }

    private async Task<ApiResponse<CredentialDetailDto>> GetGlobalAsync(int id, CancellationToken cancellationToken)
    {
        var credential = await _repository.GetGlobalByIdAsync(id, cancellationToken);
        return credential is null
            ? ApiResponse<CredentialDetailDto>.Fail([CredentialErrorMessages.GlobalCredentialNotFound], ApiStatusCodes.NotFound)
            : ApiResponse<CredentialDetailDto>.Ok(CredentialMapper.ToDetail(credential));
    }

    private async Task<ApiResponse<CredentialDetailDto>> GetTenantAsync(Guid id, CancellationToken cancellationToken)
    {
        var credential = await _repository.GetTenantByIdAsync(id, cancellationToken);
        return credential is null
            ? ApiResponse<CredentialDetailDto>.Fail([CredentialErrorMessages.TenantCredentialNotFound], ApiStatusCodes.NotFound)
            : ApiResponse<CredentialDetailDto>.Ok(CredentialMapper.ToDetail(credential));
    }
}
