using Fgs.Foundation.Compression.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fgs.Foundation.Tests.Compression;

public sealed class ResponseCompressionExtensionsTests
{
    [Fact]
    public void AddFgsResponseCompression_WhenEnabled_RegistersCompressionProviders()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddFgsResponseCompression(configuration);

        using var provider = services.BuildServiceProvider();
        provider.GetServices<IResponseCompressionProvider>().Should().NotBeEmpty();
        provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.ResponseCompression.ResponseCompressionOptions>>()
            .Should()
            .NotBeNull();
        provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProviderOptions>>()
            .Should()
            .NotBeNull();
        provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>>()
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void AddFgsResponseCompression_WhenDisabled_DoesNotRegisterCompressionProviders()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ResponseCompression:Enabled"] = "false"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddFgsResponseCompression(configuration);

        using var provider = services.BuildServiceProvider();
        provider.GetServices<IResponseCompressionProvider>().Should().BeEmpty();
    }

    [Fact]
    public async Task UseFgsResponseCompression_ReturnsGzipEncodedJson_WhenClientAcceptsGzip()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>());
        builder.Services.AddFgsResponseCompression(builder.Configuration);

        var app = builder.Build();
        app.UseFgsResponseCompression();
        app.MapGet("/api/data", () => Results.Json(new
        {
            items = Enumerable.Range(1, 200).Select(i => new
            {
                id = i,
                name = $"Item {i}",
                description = new string('x', 80)
            })
        }));

        await app.StartAsync();

        try
        {
            using var client = new HttpClient();
            var address = app.Urls.First();
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{address}/api/data");
            request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip");

            using var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Content.Headers.ContentEncoding.Should().Contain("gzip");
        }
        finally
        {
            await app.StopAsync();
        }
    }
}
