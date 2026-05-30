using System.Text.Json;
using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.UpdateCredential;

public sealed record UpdateCredentialCommand : IRequest<ApiResponse<CredentialSecretMetadataDto>>
{
    public Guid SecretId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public JsonElement? SecretPayload { get; init; }

    public string? UpdatedBy { get; init; }
}
