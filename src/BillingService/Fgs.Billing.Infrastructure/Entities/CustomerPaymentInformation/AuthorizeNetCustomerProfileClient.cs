using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;

namespace Fgs.Billing.Infrastructure.Entities.CustomerPaymentInformation;

public sealed class AuthorizeNetCustomerProfileClient(HttpClient httpClient) : IAuthorizeNetCustomerProfileClient
{
    private static readonly XNamespace Ns = "AnetApi/xml/v1/schema/AnetApiSchema.xsd";

    public Task<AuthorizeNetCallResult> CreateCustomerProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string merchantCustomerId,
        CancellationToken cancellationToken = default)
    {
        var body = new XElement(
            Ns + "createCustomerProfileRequest",
            MerchantAuthentication(settings),
            new XElement(
                Ns + "profile",
                new XElement(Ns + "merchantCustomerId", merchantCustomerId)));
        return PostAsync(settings, body, cancellationToken);
    }

    public Task<AuthorizeNetCallResult> GetCustomerProfileByMerchantCustomerIdAsync(
        CompanyAuthorizeNetSettings settings,
        string merchantCustomerId,
        CancellationToken cancellationToken = default)
    {
        var body = new XElement(
            Ns + "getCustomerProfileRequest",
            MerchantAuthentication(settings),
            new XElement(Ns + "merchantCustomerId", merchantCustomerId));
        return PostAsync(settings, body, cancellationToken);
    }

    public Task<AuthorizeNetCallResult> CreateCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        AuthorizeNetOpaqueData opaqueData,
        string cardHolderName,
        string billingZip,
        CancellationToken cancellationToken = default)
    {
        var (firstName, lastName) = SplitName(cardHolderName);
        var body = new XElement(
            Ns + "createCustomerPaymentProfileRequest",
            MerchantAuthentication(settings),
            new XElement(Ns + "customerProfileId", customerProfileId),
            new XElement(
                Ns + "paymentProfile",
                BillTo(firstName, lastName, billingZip),
                new XElement(
                    Ns + "payment",
                    new XElement(
                        Ns + "opaqueData",
                        new XElement(Ns + "dataDescriptor", opaqueData.DataDescriptor),
                        new XElement(Ns + "dataValue", opaqueData.DataValue)))),
            new XElement(Ns + "validationMode", ValidationMode(settings)));
        return PostAsync(settings, body, cancellationToken);
    }

    public Task<AuthorizeNetCallResult> GetCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        string customerPaymentProfileId,
        CancellationToken cancellationToken = default)
    {
        var body = new XElement(
            Ns + "getCustomerPaymentProfileRequest",
            MerchantAuthentication(settings),
            new XElement(Ns + "customerProfileId", customerProfileId),
            new XElement(Ns + "customerPaymentProfileId", customerPaymentProfileId),
            new XElement(Ns + "unmaskExpirationDate", "true"));
        return PostAsync(settings, body, cancellationToken);
    }

    public Task<AuthorizeNetCallResult> UpdateCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        string customerPaymentProfileId,
        string cardHolderName,
        short expirationMonth,
        short expirationYear,
        string? last4,
        AuthorizeNetOpaqueData? opaqueData,
        CancellationToken cancellationToken = default)
    {
        var (firstName, lastName) = SplitName(cardHolderName);
        XElement payment = opaqueData is null
            ? new XElement(
                Ns + "payment",
                new XElement(
                    Ns + "creditCard",
                    new XElement(Ns + "cardNumber", "XXXX" + last4),
                    new XElement(Ns + "expirationDate", FormatExpiration(expirationMonth, expirationYear))))
            : new XElement(
                Ns + "payment",
                new XElement(
                    Ns + "opaqueData",
                    new XElement(Ns + "dataDescriptor", opaqueData.DataDescriptor),
                    new XElement(Ns + "dataValue", opaqueData.DataValue)));

        var body = new XElement(
            Ns + "updateCustomerPaymentProfileRequest",
            MerchantAuthentication(settings),
            new XElement(Ns + "customerProfileId", customerProfileId),
            new XElement(
                Ns + "paymentProfile",
                BillTo(firstName, lastName, billingZip: null),
                payment,
                new XElement(Ns + "customerPaymentProfileId", customerPaymentProfileId)),
            new XElement(Ns + "validationMode", opaqueData is null ? "none" : ValidationMode(settings)));
        return PostAsync(settings, body, cancellationToken);
    }

    public Task<AuthorizeNetCallResult> DeleteCustomerPaymentProfileAsync(
        CompanyAuthorizeNetSettings settings,
        string customerProfileId,
        string customerPaymentProfileId,
        CancellationToken cancellationToken = default)
    {
        var body = new XElement(
            Ns + "deleteCustomerPaymentProfileRequest",
            MerchantAuthentication(settings),
            new XElement(Ns + "customerProfileId", customerProfileId),
            new XElement(Ns + "customerPaymentProfileId", customerPaymentProfileId));
        return PostAsync(settings, body, cancellationToken);
    }

    private async Task<AuthorizeNetCallResult> PostAsync(
        CompanyAuthorizeNetSettings settings,
        XElement body,
        CancellationToken cancellationToken)
    {
        var document = new XDocument(new XDeclaration("1.0", "utf-8", null), body);
        using var writer = new Utf8StringWriter();
        document.Save(writer);
        using var content = new StringContent(writer.ToString(), Encoding.UTF8);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/xml") { CharSet = "utf-8" };

        try
        {
            using var response = await httpClient.PostAsync(
                AuthorizeNetEndpoints.CimUrl(settings.UseSandbox),
                content,
                cancellationToken);
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(payload))
            {
                return AuthorizeNetCallResult.Transport();
            }

            return Parse(payload);
        }
        catch (HttpRequestException)
        {
            return AuthorizeNetCallResult.Transport();
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return AuthorizeNetCallResult.Transport();
        }
    }

    private static AuthorizeNetCallResult Parse(string payload)
    {
        var start = payload.IndexOf('<');
        if (start < 0)
        {
            return AuthorizeNetCallResult.Transport();
        }

        XDocument document;
        try
        {
            document = XDocument.Parse(payload[start..]);
        }
        catch (System.Xml.XmlException)
        {
            return AuthorizeNetCallResult.Transport();
        }

        var resultCode = Value(document, "resultCode") ?? "Error";
        var messageCode = Value(document, "code");
        var cardNumber = Value(document, "cardNumber");
        var expiration = Value(document, "expirationDate");
        TryParseExpiration(expiration, out var month, out var year);

        return new AuthorizeNetCallResult(
            false,
            resultCode,
            messageCode,
            Value(document, "customerProfileId"),
            Value(document, "customerPaymentProfileId"),
            Value(document, "cardType"),
            Last4(cardNumber),
            month,
            year);
    }

    private static XElement MerchantAuthentication(CompanyAuthorizeNetSettings settings) =>
        new(
            Ns + "merchantAuthentication",
            new XElement(Ns + "name", settings.ApiLoginId),
            new XElement(Ns + "transactionKey", settings.TransactionKey));

    private static XElement BillTo(string? firstName, string? lastName, string? billingZip)
    {
        var billTo = new XElement(Ns + "billTo");
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            billTo.Add(new XElement(Ns + "firstName", Truncate(firstName, 50)));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            billTo.Add(new XElement(Ns + "lastName", Truncate(lastName, 50)));
        }

        if (!string.IsNullOrWhiteSpace(billingZip))
        {
            billTo.Add(new XElement(Ns + "zip", Truncate(billingZip.Trim(), 20)));
        }

        return billTo;
    }

    private static string ValidationMode(CompanyAuthorizeNetSettings settings) =>
        settings.UseSandbox ? "testMode" : "liveMode";

    private static string FormatExpiration(short month, short year)
    {
        var fullYear = year < 100 ? 2000 + year : year;
        return $"{fullYear:0000}-{month:00}";
    }

    private static bool TryParseExpiration(string? value, out short? month, out short? year)
    {
        month = null;
        year = null;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var parts = value.Split('-', '/');
        if (parts.Length != 2)
        {
            return false;
        }

        if (parts[0].Length == 4
            && short.TryParse(parts[0], out var parsedYear)
            && short.TryParse(parts[1], out var parsedMonth)
            && parsedMonth is >= 1 and <= 12)
        {
            year = parsedYear;
            month = parsedMonth;
            return true;
        }

        if (parts[1].Length == 4
            && short.TryParse(parts[0], out parsedMonth)
            && short.TryParse(parts[1], out parsedYear)
            && parsedMonth is >= 1 and <= 12)
        {
            year = parsedYear;
            month = parsedMonth;
            return true;
        }

        return false;
    }

    private static string? Last4(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            return null;
        }

        var digits = new string(cardNumber.Where(char.IsDigit).ToArray());
        return digits.Length >= 4 ? digits[^4..] : null;
    }

    private static string? Value(XDocument document, string localName) =>
        document.Descendants().FirstOrDefault(element => element.Name.LocalName == localName)?.Value;

    private static (string? FirstName, string? LastName) SplitName(string cardHolderName)
    {
        var trimmed = cardHolderName.Trim();
        var space = trimmed.LastIndexOf(' ');
        if (space <= 0)
        {
            return (trimmed, null);
        }

        return (trimmed[..space], trimmed[(space + 1)..]);
    }

    private static string Truncate(string value, int maxLength) =>
        value.Length <= maxLength ? value : value[..maxLength];

    private sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
