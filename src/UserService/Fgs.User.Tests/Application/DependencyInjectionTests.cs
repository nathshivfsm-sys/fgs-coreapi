using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.User.Application;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Database.Read;
using Fgs.User.Infrastructure.Persistence.Read;
using Fgs.User.Infrastructure.Persistence.Write;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddFgsUserApplication_RegistersMediatRAndValidators()
    {
        var services = new ServiceCollection();
        services.AddFgsUserApplication();

        services.Should().Contain(sd =>
            sd.ServiceType.Name.Contains("ISignupUniquenessValidator", StringComparison.Ordinal));
        services.Should().Contain(sd =>
            sd.ImplementationType == typeof(SignupUniquenessValidator));
    }

    [Fact]
    public void OpenGenericRepositories_ResolveForSampleEntity()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:FgsUser"] = "Host=localhost;Database=fgs_user_test",
                ["ConnectionStrings:FgsUserReadOnly"] = "Host=localhost;Database=fgs_user_test"
            })
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ITenantContextAccessor>(new DesignTimeTenantContextAccessor());
        services.AddDbContext<FgsUserDbContext>(options =>
            options.UseInMemoryDatabase($"fgs-user-di-{Guid.NewGuid():N}"));
        services.AddFgsPersistence<FgsUserDbContext>();
        services.AddSingleton<IUserReadConnectionFactory>(_ =>
            Mock.Of<IUserReadConnectionFactory>());
        services.AddScoped(typeof(IUserReadRepository<>), typeof(UserDapperReadRepository<>));
        services.AddScoped(typeof(IUserWriteRepository<>), typeof(UserEfWriteRepository<>));

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        scope.ServiceProvider.GetRequiredService<IUserReadRepository<FgsTenant>>().Should().NotBeNull();
        scope.ServiceProvider.GetRequiredService<IUserWriteRepository<FgsTenant>>().Should().NotBeNull();
    }

    [Fact]
    public void AddFgsUserInfrastructure_RegistersOpenGenericRepositories()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:FgsUser"] = "Host=localhost;Database=fgs_user_test",
            ["ConnectionStrings:FgsUserReadOnly"] = "Host=localhost;Database=fgs_user_test",
            ["Credentials:BaseUrl"] = "http://localhost:5071",
            ["Credentials:ApiKey"] = "test-key",
            ["EntraExternalId:Authority"] = "https://login.microsoftonline.com",
            ["EntraExternalId:TenantId"] = "tenant",
            ["EntraExternalId:ClientId"] = "client",
            ["EntraExternalId:ClientSecret"] = "secret",
            ["AwsCredentials:AccessKeyId"] = "key",
            ["AwsCredentials:SecretAccessKey"] = "secret",
            ["AwsCredentials:Region"] = "us-east-1"
        });

        services.AddFgsUserInfrastructure(configuration);

        services.Should().Contain(sd =>
            sd.ServiceType == typeof(IUserReadRepository<>) &&
            sd.ImplementationType == typeof(UserDapperReadRepository<>));
        services.Should().Contain(sd =>
            sd.ServiceType == typeof(IUserWriteRepository<>) &&
            sd.ImplementationType == typeof(UserEfWriteRepository<>));
    }
}
