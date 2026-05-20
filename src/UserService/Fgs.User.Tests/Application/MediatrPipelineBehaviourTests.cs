using Fgs.User.Application.Behaviours;
using Fgs.User.Application.Features.Auth.Queries.EntraCallback;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class MediatrPipelineBehaviourTests
{
    [Fact]
    public async Task ValidationBehaviour_WhenValidationFails_ThrowsValidationException()
    {
        var validators = new IValidator<EntraCallbackQuery>[] { new EntraCallbackQueryValidator() };
        var behaviour = new ValidationBehaviour<EntraCallbackQuery, string>(validators);

        var act = () => behaviour.Handle(
            new EntraCallbackQuery(string.Empty, string.Empty),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ValidationBehaviour_WhenNoValidators_CallsNext()
    {
        var behaviour = new ValidationBehaviour<EntraCallbackQuery, string>([]);

        var result = await behaviour.Handle(
            new EntraCallbackQuery("code", Guid.NewGuid().ToString()),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        result.Should().Be("ok");
    }

    [Fact]
    public async Task LoggingBehaviour_LogsAndReturnsResponse()
    {
        var logger = new Mock<ILogger<LoggingBehaviour<EntraCallbackQuery, string>>>();
        var behaviour = new LoggingBehaviour<EntraCallbackQuery, string>(logger.Object);

        var result = await behaviour.Handle(
            new EntraCallbackQuery("code", Guid.NewGuid().ToString()),
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
    public async Task LoggingBehaviour_WhenNextThrows_Rethrows()
    {
        var logger = new Mock<ILogger<LoggingBehaviour<EntraCallbackQuery, string>>>();
        var behaviour = new LoggingBehaviour<EntraCallbackQuery, string>(logger.Object);

        var act = () => behaviour.Handle(
            new EntraCallbackQuery("code", Guid.NewGuid().ToString()),
            _ => throw new InvalidOperationException("boom"),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
