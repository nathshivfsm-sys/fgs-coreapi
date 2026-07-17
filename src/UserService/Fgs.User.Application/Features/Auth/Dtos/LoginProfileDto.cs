using Fgs.Contracts.Auth;
using Fgs.User.Application.Abstractions.Identity;

namespace Fgs.User.Application.Features.Auth.Dtos;

public sealed record LoginProfileDto(
    string AccessToken,
    string? RefreshToken,
    string? IdToken,
    int ExpiresIn,
    string TokenType,
    LoginUserDto User);

public sealed record LoginUserDto(
    Guid UserId,
    long TenantId,
    long CompanyId,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> DataAccess,
    IReadOnlyList<PublicEndpointAuthDto> PublicEndpoints);

public static class LoginProfileFactory
{
    public static LoginProfileDto FromTokensAndProfile(
        EntraTokenResult tokens,
        Fgs.User.Application.Abstractions.Identity.FgsUserProfile profile,
        string? displayName = null)
    {
        var (firstName, lastName) = Fgs.User.Application.Abstractions.Identity.LoginDisplayNameParser.Split(
            displayName ?? profile.Email);

        if (string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(tokens.DisplayName))
        {
            (firstName, lastName) = Fgs.User.Application.Abstractions.Identity.LoginDisplayNameParser.Split(
                tokens.DisplayName);
        }

        return new LoginProfileDto(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.IdToken,
            tokens.ExpiresIn,
            string.IsNullOrWhiteSpace(tokens.TokenType) ? "Bearer" : tokens.TokenType,
            new LoginUserDto(
                profile.UserId,
                profile.TenantId,
                profile.CompanyId,
                firstName,
                lastName,
                profile.Email,
                profile.Roles,
                profile.Permissions,
                profile.DataAccess,
                profile.PublicEndpoints));
    }
}
