using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation;

public static class CustomerPaymentInformationGrid
{
    public static CustomerPaymentInformationGridDto FromCard(CustomerPaymentInformationCard card) =>
        Create(
            card.Id,
            card.CardBrand,
            card.CardHolderName,
            card.Last4,
            card.ExpirationMonth,
            card.ExpirationYear,
            card.IsDefault,
            card.IsActive);

    public static CustomerPaymentInformationGridDto Create(
        long id,
        string? cardBrand,
        string? cardHolderName,
        string? last4,
        short? expirationMonth,
        short? expirationYear,
        bool isDefault,
        bool isActive) =>
        new(
            id,
            cardBrand,
            cardHolderName,
            Mask(last4),
            FormatExpiry(expirationMonth, expirationYear),
            isDefault,
            isActive);

    public static string Mask(string? last4) =>
        string.IsNullOrWhiteSpace(last4)
            ? "**** **** ****"
            : $"**** **** **** {last4.Trim()}";

    public static string? FormatExpiry(short? month, short? year)
    {
        if (month is null || year is null)
        {
            return null;
        }

        return $"{month.Value:00}/{year.Value % 100:00}";
    }
}
