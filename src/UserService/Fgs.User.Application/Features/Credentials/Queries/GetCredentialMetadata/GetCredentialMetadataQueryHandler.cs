using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredentialMetadata;

public sealed class GetCredentialMetadataQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCredentialMetadataQuery, ApiResponse<CredentialSecretMetadataDto>>
{
    public async Task<ApiResponse<CredentialSecretMetadataDto>> Handle(
        GetCredentialMetadataQuery request,
        CancellationToken cancellationToken)
    {
        var secret = await unitOfWork.Repository<FgsCredentialSecret>()
            .GetByIdAsync(request.SecretId, cancellationToken);

        if (secret is null
            || secret.TenantId != request.TenantId
            || secret.CompanyId != request.CompanyId)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.SecretNotFound],
                ApiStatusCodes.NotFound);
        }

        var provider = await unitOfWork.Repository<FgsCredentialProvider>()
            .GetByIdAsync(secret.CredentialProviderId, cancellationToken);

        if (provider is null)
        {
            return ApiResponse<CredentialSecretMetadataDto>.Fail(
                [CredentialErrorMessages.ProviderNotFound],
                ApiStatusCodes.NotFound);
        }

        var providerType = await unitOfWork.Repository<GloCredentialProviderType>()
            .FirstOrDefaultAsync(p => p.Id == provider.CredentialProviderTypeId, cancellationToken);

        return ApiResponse<CredentialSecretMetadataDto>.Ok(
            CredentialMetadataMapper.ToSecretMetadata(secret, provider, providerType?.Code));
    }
}
