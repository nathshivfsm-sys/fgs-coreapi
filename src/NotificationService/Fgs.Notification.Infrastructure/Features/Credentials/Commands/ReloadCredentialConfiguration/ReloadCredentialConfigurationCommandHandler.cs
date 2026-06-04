using Fgs.Contracts.Api;
using Fgs.Notification.Application.Features.Credentials.Commands.ReloadCredentialConfiguration;
using Fgs.Setup.Application.Abstractions.Credentials;
using MediatR;

namespace Fgs.Notification.Infrastructure.Features.Credentials.Commands.ReloadCredentialConfiguration;

public sealed class ReloadCredentialConfigurationCommandHandler(ICredentialConfigurationProvider provider)
    : IRequestHandler<ReloadCredentialConfigurationCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        ReloadCredentialConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        await provider.ReloadAsync(cancellationToken);
        return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
    }
}
