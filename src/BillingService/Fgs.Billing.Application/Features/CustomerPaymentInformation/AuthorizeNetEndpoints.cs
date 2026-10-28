namespace Fgs.Billing.Application.Features.CustomerPaymentInformation;

public static class AuthorizeNetEndpoints
{
    public const string SandboxAcceptJsUrl = "https://jstest.authorize.net/v1/Accept.js";

    public const string ProductionAcceptJsUrl = "https://js.authorize.net/v1/Accept.js";

    public const string SandboxCimUrl = "https://apitest.authorize.net/xml/v1/request.api";

    public const string ProductionCimUrl = "https://api.authorize.net/xml/v1/request.api";

    public static string AcceptJsUrl(bool useSandbox) =>
        useSandbox ? SandboxAcceptJsUrl : ProductionAcceptJsUrl;

    public static string CimUrl(bool useSandbox) =>
        useSandbox ? SandboxCimUrl : ProductionCimUrl;
}
