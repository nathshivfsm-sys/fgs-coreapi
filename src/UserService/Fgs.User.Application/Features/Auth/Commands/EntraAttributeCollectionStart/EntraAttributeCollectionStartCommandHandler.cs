using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Security;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.EntraAttributeCollectionStart;

/// <summary>
/// Prefills Entra External ID "Add details" Display Name from the signup contact name
/// stored on <c>FgsUser.DisplayName</c>.
/// </summary>
public sealed class EntraAttributeCollectionStartCommandHandler(
    IFgsUserProfileResolver profileResolver,
    IEmailNormalizer emailNormalizer)
    : IRequestHandler<EntraAttributeCollectionStartCommand, EntraAttributeCollectionStartResponseDto>
{
    public async Task<EntraAttributeCollectionStartResponseDto> Handle(
        EntraAttributeCollectionStartCommand request,
        CancellationToken cancellationToken)
    {
        var email = ExtractSignupEmail(request.Request);
        if (string.IsNullOrWhiteSpace(email))
        {
            return EntraAttributeCollectionStartResponseDto.Continue();
        }

        var normalizedEmail = emailNormalizer.Normalize(email);
        var profile = await profileResolver.ResolveBySignupEmailAsync(normalizedEmail, cancellationToken);
        if (profile is null || string.IsNullOrWhiteSpace(profile.DisplayName))
        {
            return EntraAttributeCollectionStartResponseDto.Continue();
        }

        return EntraAttributeCollectionStartResponseDto.PrefillDisplayName(profile.DisplayName.Trim());
    }

    private static string? ExtractSignupEmail(EntraAttributeCollectionStartRequestDto request)
    {
        var identities = request.Data?.UserSignUpInfo?.Identities;
        if (identities is null || identities.Count == 0)
        {
            return null;
        }

        var emailIdentity = identities.FirstOrDefault(i =>
            IsEmailSignInType(i.SignInType)
            && !string.IsNullOrWhiteSpace(i.IssuerAssignedId));

        return emailIdentity?.IssuerAssignedId
            ?? identities.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i.IssuerAssignedId))?.IssuerAssignedId;
    }

    private static bool IsEmailSignInType(string? signInType) =>
        string.Equals(signInType, "email", StringComparison.OrdinalIgnoreCase)
        || string.Equals(signInType, "emailAddress", StringComparison.OrdinalIgnoreCase);
}
