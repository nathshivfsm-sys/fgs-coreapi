using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Abstractions.Credentials;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Application.Features.Credentials.Queries.GetResolvedCredentialConfiguration;

public sealed class GetResolvedCredentialConfigurationQueryHandler(
    ICredentialConfigurationProvider configurationProvider,
    ICredentialAuditRecorder auditRecorder,
    IOptions<CredentialDistributionOptions> distributionOptions,
    IHostEnvironment environment)
    : IRequestHandler<GetResolvedCredentialConfigurationQuery, ApiResponse<ResolvedCredentialConfigurationDto>>
{
    public async Task<ApiResponse<ResolvedCredentialConfigurationDto>> Handle(
        GetResolvedCredentialConfigurationQuery request,
        CancellationToken cancellationToken)
    {
        var requestingService = string.IsNullOrWhiteSpace(request.RequestingServiceName)
            ? "unknown"
            : request.RequestingServiceName.Trim();

        if (!IsInternalServiceAuthorized(request.InternalServiceKey, distributionOptions.Value))
        {
            RecordAccessAudit(
                requestingService,
                CredentialAuditActions.SecretAccessDenied,
                "Internal service key validation failed.");
            return ApiResponse<ResolvedCredentialConfigurationDto>.Fail(
                ["Unauthorized."],
                ApiStatusCodes.Unauthorized);
        }

        if (configurationProvider.Values.Count == 0)
        {
            RecordAccessAudit(
                requestingService,
                CredentialAuditActions.SecretAccessDenied,
                "Resolved credential configuration is not loaded yet.");
            return ApiResponse<ResolvedCredentialConfigurationDto>.Fail(
                ["Resolved credential configuration is not loaded yet."],
                503);
        }

        RecordAccessAudit(
            requestingService,
            CredentialAuditActions.SecretAccessed,
            $"Resolved configuration snapshot; EntryCount={configurationProvider.Values.Count}");

        return ApiResponse<ResolvedCredentialConfigurationDto>.Ok(
            new ResolvedCredentialConfigurationDto(configurationProvider.Values));
    }

    private static bool IsInternalServiceAuthorized(
        string? providedKey,
        CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.InternalServiceKey))
        {
            return false;
        }

        return string.Equals(providedKey, options.InternalServiceKey, StringComparison.Ordinal);
    }

    private void RecordAccessAudit(
        string requestingService,
        string actionType,
        string remarks) =>
        _ = auditRecorder.RecordAsync(
            new RecordCredentialAuditRequest(
                TenantId: 0,
                CompanyId: 0,
                CredentialId: Guid.Empty,
                ActionType: actionType,
                Remarks: $"Service={requestingService}; Environment={environment.EnvironmentName}; {remarks}",
                CreatedBy: requestingService),
            CancellationToken.None);
}
