using System.Reflection;
using Fgs.User.Application.Features.Credentials.DTOs;

namespace Fgs.User.Tests.Application.Features.Credentials;

public sealed class CredentialMetadataDtoSecurityTests
{
    [Fact]
    public void CredentialSecretMetadataDto_HasNoSensitivePropertyNames()
    {
        var forbidden = new[] { "password", "secretkey", "webhooksecret", "token", "secretvalue" };
        var properties = typeof(CredentialSecretMetadataDto)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name.ToLowerInvariant());

        properties.Should().NotContain(p => forbidden.Contains(p));
    }

    [Fact]
    public void CredentialProviderMetadataDto_HasNoSensitivePropertyNames()
    {
        var forbidden = new[] { "password", "secretkey", "secret", "webhooksecret" };
        var properties = typeof(CredentialProviderMetadataDto)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name.ToLowerInvariant());

        properties.Should().NotContain(p => forbidden.Any(f => p.Contains(f, StringComparison.Ordinal)));
    }
}
