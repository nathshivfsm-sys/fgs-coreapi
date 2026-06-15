using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Security;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;

public sealed class EntraApiConnectorCommandHandler(
    IFgsUserProfileResolver profileResolver,
    IEmailNormalizer emailNormalizer)
    : IRequestHandler<EntraApiConnectorCommand, EntraApiConnectorResponseDto>
{
    private const string ConnectorVersion = "1.0.0";
    private const string ContinueAction = "Continue";
    private const string BlockAction = "ShowBlockPage";
    private const string BlockMessage = "Sign-in is not available for this account. Use the invitation link from your signup email.";

    public async Task<EntraApiConnectorResponseDto> Handle(
        EntraApiConnectorCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = string.IsNullOrWhiteSpace(request.Email)
            ? null
            : emailNormalizer.Normalize(request.Email);

        var profile = await profileResolver.ResolveForEntraConnectorAsync(
            request.ObjectId,
            normalizedEmail,
            cancellationToken);

        if (profile is null)
        {
            return new EntraApiConnectorResponseDto(
                ConnectorVersion,
                BlockAction,
                UserMessage: BlockMessage);
        }

        return new EntraApiConnectorResponseDto(
            ConnectorVersion,
            ContinueAction,
            profile.TenantId.ToString(),
            profile.CompanyId.ToString());
    }
}
