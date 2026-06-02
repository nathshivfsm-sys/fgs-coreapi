using System.Text;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Features.Credentials.Commands.CreateCredential;
using Fgs.User.Application.Features.Credentials.Services;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Persistence.Database.UnitOfWorks;
using Fgs.User.Tests;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class CreateCredentialCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatesGlobalCredential_WhenProviderExists()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var unitOfWork = new UnitOfWork(context);
        var providerType = new GloCredentialProviderType
        {
            Id = 1,
            ProviderCode = "RABBITMQ",
            ProviderName = "RabbitMQ",
            ConfigurationSchema = "{}",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.GloCredentialProviderTypes.Add(providerType);
        await context.SaveChangesAsync();

        var repository = new Fgs.User.Infrastructure.Persistence.Database.Repositories.CredentialRepository(context);
        var encryption = new Mock<ICredentialEncryptionService>();
        encryption.Setup(x => x.EncryptAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EnvelopeEncryptionResult([1, 2, 3], [4, 5, 6], "arn:aws:kms:test"));

        var mutationService = new CredentialMutationService(
            repository,
            encryption.Object,
            new TestActorResolver(),
            new TestDateTimeProvider(),
            unitOfWork,
            new TestConfigurationProvider());

        var handler = new CreateCredentialCommandHandler(mutationService);
        var response = await handler.Handle(
            new CreateCredentialCommand(
                CredentialScope.Global,
                "RABBITMQ",
                "Primary RabbitMQ",
                "{\"HostName\":\"localhost\"}"),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        context.GloCredentials.Should().ContainSingle();
    }

    private sealed class TestActorResolver : ICredentialActorResolver
    {
        public string ResolveActorId() => "test-user";
    }

    private sealed class TestDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }

    private sealed class TestConfigurationProvider : ICredentialConfigurationProvider
    {
        public IReadOnlyDictionary<string, string> Values { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string? GetValue(string key) => null;

        public Task ReloadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
