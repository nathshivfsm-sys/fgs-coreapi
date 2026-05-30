using Fgs.Persistence.Abstractions;
using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.ListCredentialProviders;

public sealed class ListCredentialProvidersQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ListCredentialProvidersQuery, ApiResponse<IReadOnlyList<CredentialProviderMetadataDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<CredentialProviderMetadataDto>>> Handle(
        ListCredentialProvidersQuery request,
        CancellationToken cancellationToken)
    {
        var providers = await unitOfWork.Repository<FgsCredentialProvider>()
            .ListAsync(
                p => p.TenantId == request.TenantId && p.CompanyId == request.CompanyId,
                cancellationToken);

        var typeIds = providers.Select(p => p.CredentialProviderTypeId).Distinct().ToList();
        var types = await unitOfWork.Repository<GloCredentialProviderType>()
            .ListAsync(t => typeIds.Contains(t.Id), cancellationToken);

        var typeById = types.ToDictionary(t => t.Id);

        var dtos = providers
            .Select(p => CredentialMetadataMapper.ToProviderMetadata(
                p,
                typeById.TryGetValue(p.CredentialProviderTypeId, out var t) ? t.Code : null))
            .ToList();

        return ApiResponse<IReadOnlyList<CredentialProviderMetadataDto>>.Ok(dtos);
    }
}
