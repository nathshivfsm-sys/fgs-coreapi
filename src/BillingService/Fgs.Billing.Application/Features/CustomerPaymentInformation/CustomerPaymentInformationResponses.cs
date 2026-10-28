using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation;

internal static class CustomerPaymentInformationResponses
{
    public const int BadGateway = 502;

    public static ApiResponse<T> MissingSettings<T>() =>
        ApiResponse<T>.Fail(
            ["Authorize.net settings were not found for this company."],
            ApiStatusCodes.NotFound);

    public static ApiResponse<T> Unavailable<T>() =>
        ApiResponse<T>.Fail(
            ["The payment provider is unavailable."],
            BadGateway);

    public static ApiResponse<T> Rejected<T>() =>
        ApiResponse<T>.Fail(
            ["The payment provider rejected the request."],
            ApiStatusCodes.BadRequest);

    public static ApiResponse<T> NotFound<T>() =>
        ApiResponse<T>.Fail(
            ["Saved card was not found."],
            ApiStatusCodes.NotFound);

    public static ApiResponse<T> SaveFailed<T>() =>
        ApiResponse<T>.Fail(
            ["The card could not be saved."],
            ApiStatusCodes.InternalServerError);

    public static ApiResponse<T> IncompleteCard<T>() =>
        ApiResponse<T>.Fail(
            ["The payment provider did not return card details."],
            ApiStatusCodes.InternalServerError);

    public static bool TryGetSettings<T>(
        CompanyAuthorizeNetSettingsResult result,
        out CompanyAuthorizeNetSettings settings,
        out ApiResponse<T>? failure)
    {
        if (result.TransportFailure || result.Settings is null)
        {
            settings = null!;
            failure = result.TransportFailure ? Unavailable<T>() : MissingSettings<T>();
            return false;
        }

        settings = result.Settings;
        failure = null;
        return true;
    }

    public static ApiResponse<T>? MapProviderFailure<T>(AuthorizeNetCallResult result)
    {
        if (result.IsOk)
        {
            return null;
        }

        return result.TransportFailure ? Unavailable<T>() : Rejected<T>();
    }
}
