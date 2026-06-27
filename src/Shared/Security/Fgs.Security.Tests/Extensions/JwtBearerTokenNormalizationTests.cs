using Fgs.Security.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Tests.Extensions;

public sealed class JwtBearerTokenNormalizationTests
{
    [Fact]
    public void NormalizeBearerToken_ReadsFromAuthorizationHeaderWhenContextTokenEmpty()
    {
        const string graphToken =
            "eyJ0eXAiOiJKV1QiLCJub25jZSI6InRlc3Qtbm9uY2UifQ."
            + "eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAifQ."
            + "sig";

        var extracted = FgsRequestAuthContext.ExtractBearerToken(
            new DefaultHttpContext
            {
                Request =
                {
                    Headers = { Authorization = $"Bearer {graphToken}" }
                }
            });

        extracted.Should().Be(graphToken);

        var normalized = FgsEntraGraphAccessTokenNormalizer.NormalizeIfRequired(extracted!)!;
        normalized.Should().NotBe(graphToken);
        normalized.Should().EndWith(".sig");
    }
}
