using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.ListCredentialSecrets;

public sealed class ListCredentialSecretsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ListCredentialSecretsQuery, ApiResponse<IReadOnlyList<CredentialSecretMetadataDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<CredentialSecretMetadataDto>>> Handle(
        ListCredentialSecretsQuery request,
        CancellationToken cancellationToken)
    {
        var secrets = await unitOfWork.Repository<FgsCredentialSecret>()
            .ListAsync(
                s => s.TenantId == request.TenantId
                    && s.CompanyId == request.CompanyId
                    && (!request.ProviderId.HasValue || s.CredentialProviderId == request.ProviderId)
                    && (!request.ActiveOnly || (s.IsActive && !s.IsRevoked)),
                cancellationToken);

        if (secrets.Count == 0)
        {
            return ApiResponse<IReadOnlyList<CredentialSecretMetadataDto>>.Ok([]);
        }

        var providerIds = secrets.Select(s => s.CredentialProviderId).Distinct().ToList();
        var providers = await unitOfWork.Repository<FgsCredentialProvider>()
            .ListAsync(p => providerIds.Contains(p.Id), cancellationToken);

        var providerById = providers.ToDictionary(p => p.Id);
        var typeIds = providers.Select(p => p.CredentialProviderTypeId).Distinct().ToList();
        var types = await unitOfWork.Repository<GloCredentialProviderType>()
            .ListAsync(t => typeIds.Contains(t.Id), cancellationToken);
        var typeById = types.ToDictionary(t => t.Id);

        var dtos = secrets
            .Where(s => providerById.ContainsKey(s.CredentialProviderId))
            .Select(s =>
            {
                var provider = providerById[s.CredentialProviderId];
                typeById.TryGetValue(provider.CredentialProviderTypeId, out var providerType);
                return CredentialMetadataMapper.ToSecretMetadata(secret: s, provider, providerType?.Code);
            })
            .ToList();

        return ApiResponse<IReadOnlyList<CredentialSecretMetadataDto>>.Ok(dtos);
    }
}
