using System.Reflection;
using Fgs.Foundation.Api;
using Fgs.Foundation.Api.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fgs.Foundation.Tests.Swagger;

public sealed class TenantScopeSwaggerRulesTests
{
    [Theory]
    [InlineData("api/v1/bff/signup/company")]
    [InlineData("api/v1/auth/me")]
    [InlineData("api/v1/login/start")]
    [InlineData("api/v1/signup/company")]
    [InlineData("api/v1/invite/start")]
    [InlineData("api/v1/internal/users")]
    [InlineData("api/v1/credential/resolved")]
    public void ShouldSkipTenantScopeHeaders_ForDefaultSkipPaths_ReturnsTrue(string relativePath)
    {
        var method = typeof(DummyController).GetMethod(nameof(DummyController.TenantScoped))!;

        TenantScopeSwaggerRules.ShouldSkipTenantScopeHeaders(
                relativePath,
                method,
                FgsTenantScopeDefaults.SkipPathPrefixes)
            .Should().BeTrue();
    }

    [Fact]
    public void ShouldSkipTenantScopeHeaders_ForTenantScopedCatalog_ReturnsFalse()
    {
        var method = typeof(DummyController).GetMethod(nameof(DummyController.TenantScoped))!;

        TenantScopeSwaggerRules.ShouldSkipTenantScopeHeaders(
                "api/v1/billingcategory/lookup",
                method,
                FgsTenantScopeDefaults.SkipPathPrefixes)
            .Should().BeFalse();
    }

    [Fact]
    public void ShouldSkipTenantScopeHeaders_ForAllowAnonymousAction_ReturnsTrue()
    {
        var method = typeof(DummyController).GetMethod(nameof(DummyController.Anonymous))!;

        TenantScopeSwaggerRules.ShouldSkipTenantScopeHeaders(
                "api/v1/billingcategory/lookup",
                method,
                FgsTenantScopeDefaults.SkipPathPrefixes)
            .Should().BeTrue();
    }

    [Fact]
    public void ResolveSkipPathPrefixes_UsesConfiguredValuesWhenPresent()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["TenantScope:SkipPathPrefixes:0"] = "/api/v1/custom-only"
            })
            .Build();

        TenantScopeSwaggerRules.ResolveSkipPathPrefixes(configuration)
            .Should().Equal("/api/v1/custom-only");
    }

    [Fact]
    public void ResolveSkipPathPrefixes_FallsBackToDefaultsWhenMissing()
    {
        var configuration = new ConfigurationBuilder().Build();

        TenantScopeSwaggerRules.ResolveSkipPathPrefixes(configuration)
            .Should().Equal(FgsTenantScopeDefaults.SkipPathPrefixes);
    }

    private sealed class DummyController : ControllerBase
    {
        [HttpGet]
        public IActionResult TenantScoped() => Ok();

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Anonymous() => Ok();
    }
}
