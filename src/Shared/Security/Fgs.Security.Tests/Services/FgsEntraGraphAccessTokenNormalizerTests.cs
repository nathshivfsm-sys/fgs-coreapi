using System.Security.Cryptography;
using System.Text;
using Fgs.Security.Services;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;

namespace Fgs.Security.Tests.Services;

public sealed class FgsEntraGraphAccessTokenNormalizerTests
{
    [Fact]
    public void NormalizeIfRequired_LeavesNonGraphAudienceTokenUnchanged()
    {
        var token = BuildToken(
            header: """{"typ":"JWT","nonce":"abc","alg":"RS256","kid":"k1"}""",
            payload: """{"aud":"3c788340-59a5-4864-b1b4-4f9adeffcb37","iss":"https://example.test/"}""");

        FgsEntraGraphAccessTokenNormalizer.NormalizeIfRequired(token).Should().Be(token);
    }

    [Fact]
    public void NormalizeIfRequired_LeavesGraphTokenWithoutNonceUnchanged()
    {
        var token = BuildToken(
            header: """{"typ":"JWT","alg":"RS256","kid":"k1"}""",
            payload: $$"""{"aud":"{{FgsEntraTokenValidation.MicrosoftGraphAudience}}","iss":"https://example.test/"}""");

        FgsEntraGraphAccessTokenNormalizer.NormalizeIfRequired(token).Should().Be(token);
    }

    [Fact]
    public void NormalizeIfRequired_ReplacesGraphTokenHeaderNonceWithSha256Base64Url()
    {
        const string nonce = "LF20skTIqgUv8x0JRtMteFxegW88K4Je634_mUQz9eE";
        var expectedHashedNonce = Base64UrlEncoder.Encode(
            SHA256.HashData(Encoding.UTF8.GetBytes(nonce)));

        var token = BuildToken(
            header: $$"""{"typ":"JWT","nonce":"{{nonce}}","alg":"RS256","kid":"k1"}""",
            payload: $$"""{"aud":"{{FgsEntraTokenValidation.MicrosoftGraphAudience}}","iss":"https://example.test/"}""");

        var normalized = FgsEntraGraphAccessTokenNormalizer.NormalizeIfRequired(token)!;
        var headerJson = Encoding.UTF8.GetString(
            Base64UrlEncoder.DecodeBytes(normalized.Split('.')[0]));

        headerJson.Should().Contain($"\"nonce\":\"{expectedHashedNonce}\"");
        headerJson.Should().NotContain(nonce);
    }

    private static string BuildToken(string header, string payload) =>
        $"{Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(header))}.{Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(payload))}.signature";
}
