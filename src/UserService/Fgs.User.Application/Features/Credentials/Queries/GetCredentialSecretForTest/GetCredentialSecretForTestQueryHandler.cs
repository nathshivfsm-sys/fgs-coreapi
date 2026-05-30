using System.Text.Json;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Application.Features.Credentials.Payloads;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredentialSecretForTest;

public sealed class GetCredentialSecretForTestQueryHandler(
    ICredentialSecretResolver resolver,
    ICredentialPayloadDeserializer payloadDeserializer,
    ICredentialConnectionStringBuilder connectionStringBuilder)
    : IRequestHandler<GetCredentialSecretForTestQuery, ApiResponse<CredentialSecretTestDto>>
{
    public async Task<ApiResponse<CredentialSecretTestDto>> Handle(
        GetCredentialSecretForTestQuery request,
        CancellationToken cancellationToken)
    {
        var resolution = await resolver.ResolveAsync(
            request.TenantId,
            request.CompanyId,
            request.SecretId,
            request.AccessedBy ?? "credential-test-endpoint",
            cancellationToken);

        if (resolution is null)
        {
            return ApiResponse<CredentialSecretTestDto>.Fail(
                [CredentialErrorMessages.SecretNotFound],
                ApiStatusCodes.NotFound);
        }

        using var doc = JsonDocument.Parse(resolution.SecretJson);
        var payload = doc.RootElement.Clone();

        string? sqlConnectionString = null;
        try
        {
            var dbPayload = payloadDeserializer.Deserialize<SqlDatabaseSecretPayload>(
                resolution.ProviderTypeCode,
                resolution.SecretJson);
            sqlConnectionString = connectionStringBuilder.BuildSqlConnectionString(dbPayload);
        }
        catch (InvalidOperationException)
        {
            // Not a SQL database payload shape.
        }

        return ApiResponse<CredentialSecretTestDto>.Ok(new CredentialSecretTestDto
        {
            SecretId = resolution.SecretId,
            ProviderTypeCode = resolution.ProviderTypeCode,
            VersionNo = resolution.VersionNo,
            SecretPayload = payload,
            SqlConnectionString = sqlConnectionString
        });
    }
}
