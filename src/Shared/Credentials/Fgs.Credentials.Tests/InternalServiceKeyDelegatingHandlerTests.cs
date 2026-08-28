using System.Net;
using Fgs.Contracts.Clients;
using Fgs.Credentials.Http;
using Fgs.Credentials.Options;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials.Tests;

public sealed class InternalServiceKeyDelegatingHandlerTests
{
    [Fact]
    public async Task SendAsync_WhenOptionsEmpty_FallsBackToConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["CredentialDistribution:InternalServiceKey"] = "from-config"
            })
            .Build();

        HttpRequestMessage? captured = null;
        var handler = new InternalServiceKeyDelegatingHandler(
            Microsoft.Extensions.Options.Options.Create(new CredentialDistributionOptions()).ToMonitor(),
            Microsoft.Extensions.Options.Options.Create(new CredentialConsumerOptions()).ToMonitor(),
            configuration)
        {
            InnerHandler = new CaptureHandler(request =>
            {
                captured = request;
                return new HttpResponseMessage(HttpStatusCode.OK);
            })
        };

        using var client = new HttpClient(handler);
        await client.GetAsync("http://localhost/test");

        captured.Should().NotBeNull();
        captured!.Headers.GetValues(InternalServiceHeaders.ServiceKey)
            .Should().ContainSingle().Which.Should().Be("from-config");
    }

    [Fact]
    public async Task SendAsync_WhenOptionsHasKey_UsesOptions()
    {
        var configuration = new ConfigurationBuilder().Build();
        HttpRequestMessage? captured = null;
        var handler = new InternalServiceKeyDelegatingHandler(
            Microsoft.Extensions.Options.Options.Create(new CredentialDistributionOptions
            {
                InternalServiceKey = "from-options"
            }).ToMonitor(),
            Microsoft.Extensions.Options.Options.Create(new CredentialConsumerOptions
            {
                ServiceName = "fgs-test"
            }).ToMonitor(),
            configuration)
        {
            InnerHandler = new CaptureHandler(request =>
            {
                captured = request;
                return new HttpResponseMessage(HttpStatusCode.OK);
            })
        };

        using var client = new HttpClient(handler);
        await client.GetAsync("http://localhost/test");

        captured.Should().NotBeNull();
        captured!.Headers.GetValues(InternalServiceHeaders.ServiceKey)
            .Should().ContainSingle().Which.Should().Be("from-options");
        captured.Headers.GetValues(InternalServiceHeaders.ServiceName)
            .Should().ContainSingle().Which.Should().Be("fgs-test");
    }

    private sealed class CaptureHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(respond(request));
    }
}

file static class OptionsMonitorExtensions
{
    public static IOptionsMonitor<T> ToMonitor<T>(this IOptions<T> options)
        where T : class =>
        new StaticOptionsMonitor<T>(options.Value);

    private sealed class StaticOptionsMonitor<T>(T value) : IOptionsMonitor<T>
        where T : class
    {
        public T CurrentValue => value;

        public T Get(string? name) => value;

        public IDisposable? OnChange(Action<T, string?> listener) => null;
    }
}
