using Fgs.Credentials.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Consumer.Application.Features.Credentials.Commands.ProcessCredentialConfigurationChanged;

/// <summary>
/// Backup path for ConsumerService when Redis pub/sub is missed.
/// API hosts still rely on Redis snapshot subscribe + periodic refresh.
/// </summary>
public sealed class ProcessCredentialConfigurationChangedCommandHandler(
    ICredentialConfigurationProvider configurationProvider,
    ILogger<ProcessCredentialConfigurationChangedCommandHandler> logger)
    : IRequestHandler<ProcessCredentialConfigurationChangedCommand>
{
    public async Task Handle(
        ProcessCredentialConfigurationChangedCommand request,
        CancellationToken cancellationToken)
    {
        await configurationProvider.ReloadAsync(cancellationToken);
        logger.LogInformation(
            "Reloaded credential configuration after CredentialConfigurationChanged at {OccurredAtUtc}.",
            request.Event.OccurredAtUtc);
    }
}
