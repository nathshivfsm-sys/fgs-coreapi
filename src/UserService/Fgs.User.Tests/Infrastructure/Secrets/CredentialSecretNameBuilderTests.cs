using Fgs.User.Infrastructure.Common.Options;
using Fgs.Security.Options;
using Fgs.User.Infrastructure.Secrets;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure.Secrets;

public sealed class CredentialSecretNameBuilderTests
{
    [Theory]
    [InlineData("prod", "tenant-001", "STRIPE", "prod/fsm/tenant-001/stripe")]
    [InlineData("prod", "tenant-001", "SendGrid", "prod/fsm/tenant-001/sendgrid")]
    [InlineData("Production", "tenant-001", "sql-write", "production/fsm/tenant-001/sql-write")]
    public void BuildSecretName_follows_environment_fsm_tenant_provider_pattern(
        string environment,
        string tenantCode,
        string providerCode,
        string expected)
    {
        var sut = new CredentialSecretNameBuilder(
            Options.Create(new AwsCredentialsOptions { ApplicationSlug = "fsm" }));

        var fullName = sut.BuildSecretName(environment, tenantCode, providerCode);
        fullName.Should().Be(expected);
        fullName[(fullName.LastIndexOf('/') + 1)..].Should().Be(expected[(expected.LastIndexOf('/') + 1)..]);
    }
}
