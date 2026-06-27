using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials.Http;

public sealed class InternalServiceKeyDelegatingHandler : DelegatingHandler
{
    private readonly IOptions<CredentialDistributionOptions> _distributionOptions;
    private readonly IOptions<CredentialConsumerOptions> _consumerOptions;

    public InternalServiceKeyDelegatingHandler(
        IOptions<CredentialDistributionOptions> distributionOptions,
        IOptions<CredentialConsumerOptions> consumerOptions)
    {
        _distributionOptions = distributionOptions;
        _consumerOptions = consumerOptions;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!HasNonEmptyHeader(request, InternalServiceHeaders.ServiceKey))
        {
            var serviceKey = _distributionOptions.Value.InternalServiceKey;
            if (!string.IsNullOrWhiteSpace(serviceKey))
            {
                request.Headers.Remove(InternalServiceHeaders.ServiceKey);
                request.Headers.TryAddWithoutValidation(InternalServiceHeaders.ServiceKey, serviceKey);
            }
        }

        if (!request.Headers.Contains(InternalServiceHeaders.ServiceName))
        {
            var serviceName = _consumerOptions.Value.ServiceName;
            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                request.Headers.TryAddWithoutValidation(InternalServiceHeaders.ServiceName, serviceName);
            }
        }

        return base.SendAsync(request, cancellationToken);
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
