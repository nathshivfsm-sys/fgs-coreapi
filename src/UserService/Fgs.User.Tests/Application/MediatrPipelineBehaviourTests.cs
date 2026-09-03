using Fgs.Contracts.Api;
using Fgs.Foundation.Behaviors;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Application.Features.Auth.Dtos;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class MediatrPipelineBehaviourTests
{
    [Fact]
    public async Task ValidationBehavior_WhenValidationFails_ThrowsValidationException()
    {
        var validators = new IValidator<ExchangeLoginCodeCommand>[] { new ExchangeLoginCodeCommandValidator() };
        var behavior = new ValidationBehavior<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>(validators);

        var act = () => behavior.Handle(
            new ExchangeLoginCodeCommand(string.Empty, string.Empty),
            () => Task.FromResult(ApiResponse<LoginProfileDto>.Ok(null!)),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ValidationBehavior_WhenNoValidators_CallsNext()
    {
        var behavior = new ValidationBehavior<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>([]);

        var expected = ApiResponse<LoginProfileDto>.Ok(null!);
        var result = await behavior.Handle(
            new ExchangeLoginCodeCommand("code", Guid.NewGuid().ToString()),
            () => Task.FromResult(expected),
            CancellationToken.None);

        result.Should().Be(expected);
    }

    [Fact]
    public async Task LoggingBehavior_LogsAndReturnsResponse()
    {
        var logger = new Mock<ILogger<LoggingBehavior<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>>>();
        var behavior = new LoggingBehavior<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>(logger.Object);

        var expected = ApiResponse<LoginProfileDto>.Ok(null!);
        var result = await behavior.Handle(
            new ExchangeLoginCodeCommand("code", Guid.NewGuid().ToString()),
            () => Task.FromResult(expected),
            CancellationToken.None);

        result.Should().Be(expected);
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
        var logger = new Mock<ILogger<LoggingBehavior<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>>>();
        var behavior = new LoggingBehavior<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>(logger.Object);

        var act = () => behavior.Handle(
            new ExchangeLoginCodeCommand("code", Guid.NewGuid().ToString()),
            () => throw new InvalidOperationException("boom"),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
