using Fgs.Foundation.Behaviors;
using Fgs.User.Application.Features.Auth.Commands.EntraCallback;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class MediatrPipelineBehaviourTests
{
    [Fact]
    public async Task ValidationBehavior_WhenValidationFails_ThrowsValidationException()
    {
        var validators = new IValidator<EntraCallbackCommand>[] { new EntraCallbackCommandValidator() };
        var behavior = new ValidationBehavior<EntraCallbackCommand, string>(validators);

        var act = () => behavior.Handle(
            new EntraCallbackCommand(string.Empty, string.Empty),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ValidationBehavior_WhenNoValidators_CallsNext()
    {
        var behavior = new ValidationBehavior<EntraCallbackCommand, string>([]);

        var result = await behavior.Handle(
            new EntraCallbackCommand("code", Guid.NewGuid().ToString()),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        result.Should().Be("ok");
    }

    [Fact]
    public async Task LoggingBehavior_LogsAndReturnsResponse()
    {
        var logger = new Mock<ILogger<LoggingBehavior<EntraCallbackCommand, string>>>();
        var behavior = new LoggingBehavior<EntraCallbackCommand, string>(logger.Object);

        var result = await behavior.Handle(
            new EntraCallbackCommand("code", Guid.NewGuid().ToString()),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        result.Should().Be("ok");
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((_, _) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task LoggingBehavior_WhenNextThrows_Rethrows()
    {
        var logger = new Mock<ILogger<LoggingBehavior<EntraCallbackCommand, string>>>();
        var behavior = new LoggingBehavior<EntraCallbackCommand, string>(logger.Object);

        var act = () => behavior.Handle(
            new EntraCallbackCommand("code", Guid.NewGuid().ToString()),
            _ => throw new InvalidOperationException("boom"),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
