using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.RevokeCredential;

public sealed record RevokeCredentialCommand : IRequest<ApiResponse<CredentialSecretMetadataDto>>
{
    public Guid SecretId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public string? RevokedBy { get; init; }
}
