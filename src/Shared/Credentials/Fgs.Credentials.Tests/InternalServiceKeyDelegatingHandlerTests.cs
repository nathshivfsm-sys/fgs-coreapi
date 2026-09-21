using System.Net;
using Fgs.Contracts.Clients;
using Fgs.Credentials.Http;
using Fgs.Credentials.Options;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

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
        var handler = CreateHandler(
            distribution: new CredentialDistributionOptions(),
            consumer: new CredentialConsumerOptions(),
            configuration: configuration,
            httpContextAccessor: null,
            onRequest: request => captured = request);

        using var client = new HttpClient(handler);
        await client.GetAsync("http://localhost/test");

        captured.Should().NotBeNull();
        captured!.Headers.GetValues(InternalServiceHeaders.ServiceKey)
            .Should().ContainSingle().Which.Should().Be("from-config");
        captured.Headers.Contains("Authorization").Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_WhenOptionsHasKey_UsesOptions()
    {
        var configuration = new ConfigurationBuilder().Build();
        HttpRequestMessage? captured = null;
        var handler = CreateHandler(
            distribution: new CredentialDistributionOptions
            {
                InternalServiceKey = "from-options"
            },
            consumer: new CredentialConsumerOptions
            {
                ServiceName = "fgs-test"
            },
            configuration: configuration,
            httpContextAccessor: null,
            onRequest: request => captured = request);

        using var client = new HttpClient(handler);
        await client.GetAsync("http://localhost/test");

        captured.Should().NotBeNull();
        captured!.Headers.GetValues(InternalServiceHeaders.ServiceKey)
            .Should().ContainSingle().Which.Should().Be("from-options");
        captured.Headers.GetValues(InternalServiceHeaders.ServiceName)
            .Should().ContainSingle().Which.Should().Be("fgs-test");
    }

    [Fact]
    public async Task SendAsync_WhenInboundBearerPresent_ForwardsTokenAndOmitsServiceKey()
    {
        var configuration = new ConfigurationBuilder().Build();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Bearer caller-jwt";

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns(httpContext);

        HttpRequestMessage? captured = null;
        var handler = CreateHandler(
            distribution: new CredentialDistributionOptions
            {
                InternalServiceKey = "should-not-send"
            },
            consumer: new CredentialConsumerOptions(),
            configuration: configuration,
            httpContextAccessor: accessor.Object,
            onRequest: request => captured = request);

        using var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");
        // Simulate DefaultRequestHeaders already stamping the service key.
        request.Headers.TryAddWithoutValidation(InternalServiceHeaders.ServiceKey, "should-not-send");
        await client.SendAsync(request);

        captured.Should().NotBeNull();
        captured!.Headers.GetValues("Authorization")
            .Should().ContainSingle().Which.Should().Be("Bearer caller-jwt");
        captured.Headers.Contains(InternalServiceHeaders.ServiceKey).Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_WhenNoInboundBearer_UsesServiceKey()
    {
        var configuration = new ConfigurationBuilder().Build();
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(a => a.HttpContext).Returns(new DefaultHttpContext());

        HttpRequestMessage? captured = null;
        var handler = CreateHandler(
            distribution: new CredentialDistributionOptions
            {
                InternalServiceKey = "s2s-key"
            },
            consumer: new CredentialConsumerOptions(),
            configuration: configuration,
            httpContextAccessor: accessor.Object,
            onRequest: request => captured = request);

        using var client = new HttpClient(handler);
        await client.GetAsync("http://localhost/test");

        captured.Should().NotBeNull();
        captured!.Headers.GetValues(InternalServiceHeaders.ServiceKey)
            .Should().ContainSingle().Which.Should().Be("s2s-key");
        captured.Headers.Contains("Authorization").Should().BeFalse();
    }

    private static InternalServiceKeyDelegatingHandler CreateHandler(
        CredentialDistributionOptions distribution,
        CredentialConsumerOptions consumer,
        IConfiguration configuration,
        IHttpContextAccessor? httpContextAccessor,
        Action<HttpRequestMessage> onRequest) =>
        new(
            Microsoft.Extensions.Options.Options.Create(distribution).ToMonitor(),
            Microsoft.Extensions.Options.Options.Create(consumer).ToMonitor(),
            configuration,
            httpContextAccessor)
        {
            InnerHandler = new CaptureHandler(request =>
            {
                onRequest(request);
                return new HttpResponseMessage(HttpStatusCode.OK);
            })
        };

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
