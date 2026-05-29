using System.Text.Json;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.CreateCredential;

public sealed class CreateCredentialCommand : IRequest<ApiResponse<CredentialSecretMetadataDto>>
{
    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public int CredentialProviderTypeId { get; init; }

    public string ProviderCode { get; init; } = null!;

    public string ProviderName { get; init; } = null!;

    public string Environment { get; init; } = "Production";

    public string? Description { get; init; }

    /// <summary>Secret name is derived from <see cref="ProviderCode"/> (not sent in the request).</summary>
    public JsonElement SecretPayload { get; init; }

    public IReadOnlyDictionary<string, string>? Configurations { get; init; }

    public string? CreatedBy { get; init; }
}
