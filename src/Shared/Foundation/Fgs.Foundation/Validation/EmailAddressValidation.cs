using System.Net.Mail;
using FluentValidation;

namespace Fgs.Foundation.Validation;

/// <summary>
/// Stricter than FluentValidation's default <c>EmailAddress()</c>, which only requires a single
/// <c>@</c> that is not first or last (ASP.NET Core compatible) and therefore accepts values like
/// <c>user@domain,com</c>.
/// </summary>
public static class EmailAddressValidation
{
    public static bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var trimmed = email.Trim();
        if (!MailAddress.TryCreate(trimmed, out var address))
        {
            return false;
        }

        // Reject display-name forms ("Name <user@host>") and require a dotted host (e.g. example.com).
        return string.Equals(address.Address, trimmed, StringComparison.OrdinalIgnoreCase)
            && address.Host.Contains('.');
    }
}

public static class EmailAddressValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValidEmailAddress<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .Must(EmailAddressValidation.IsValid)
            .WithMessage("'{PropertyName}' is not a valid email address.");
}
