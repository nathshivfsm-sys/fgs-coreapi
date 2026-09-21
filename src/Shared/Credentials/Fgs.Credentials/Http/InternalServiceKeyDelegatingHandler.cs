using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials.Http;

/// <summary>
/// Outbound S2S auth: forwards the inbound Bearer token when present; otherwise stamps the internal service key.
/// </summary>
public sealed class InternalServiceKeyDelegatingHandler : DelegatingHandler
{
    private const string AuthorizationHeader = "Authorization";

    private readonly IOptionsMonitor<CredentialDistributionOptions> _distributionOptions;
    private readonly IOptionsMonitor<CredentialConsumerOptions> _consumerOptions;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public InternalServiceKeyDelegatingHandler(
        IOptionsMonitor<CredentialDistributionOptions> distributionOptions,
        IOptionsMonitor<CredentialConsumerOptions> consumerOptions,
        IConfiguration configuration,
        IHttpContextAccessor? httpContextAccessor = null)
    {
        _distributionOptions = distributionOptions;
        _consumerOptions = consumerOptions;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (TryForwardCallerAuthorization(request))
        {
            // Prefer caller JWT over service key when both would otherwise be present
            // (DefaultRequestHeaders may already stamp the key).
            request.Headers.Remove(InternalServiceHeaders.ServiceKey);
        }
        else if (!HasNonEmptyHeader(request, InternalServiceHeaders.ServiceKey))
        {
            var serviceKey = ResolveServiceKey();
            if (!string.IsNullOrWhiteSpace(serviceKey))
            {
                request.Headers.Remove(InternalServiceHeaders.ServiceKey);
                request.Headers.TryAddWithoutValidation(InternalServiceHeaders.ServiceKey, serviceKey);
            }
        }

        if (!request.Headers.Contains(InternalServiceHeaders.ServiceName))
        {
            var serviceName = FirstNonEmpty(
                _consumerOptions.CurrentValue.ServiceName,
                _configuration[$"{CredentialConsumerOptions.SectionName}:ServiceName"],
                Environment.GetEnvironmentVariable("CredentialConsumer__ServiceName"));

            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                request.Headers.TryAddWithoutValidation(InternalServiceHeaders.ServiceName, serviceName);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }

    private bool TryForwardCallerAuthorization(HttpRequestMessage request)
    {
        if (HasNonEmptyHeader(request, AuthorizationHeader))
        {
            return true;
        }

        var inbound = _httpContextAccessor?.HttpContext?.Request.Headers[AuthorizationHeader]
            .FirstOrDefault();
        if (string.IsNullOrWhiteSpace(inbound))
        {
            return false;
        }

        request.Headers.TryAddWithoutValidation(AuthorizationHeader, inbound);
        return true;
    }

    private string? ResolveServiceKey() =>
        FirstNonEmpty(
            _distributionOptions.CurrentValue.InternalServiceKey,
            _configuration[$"{CredentialDistributionOptions.SectionName}:InternalServiceKey"],
            Environment.GetEnvironmentVariable("CredentialDistribution__InternalServiceKey"),
            Environment.GetEnvironmentVariable("CREDENTIAL_DISTRIBUTION_KEY"));

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return null;
    }

    private static bool HasNonEmptyHeader(HttpRequestMessage request, string headerName)
    {
        if (!request.Headers.TryGetValues(headerName, out var values))
        {
            return false;
        }

        return values.Any(value => !string.IsNullOrWhiteSpace(value));
    }
}
