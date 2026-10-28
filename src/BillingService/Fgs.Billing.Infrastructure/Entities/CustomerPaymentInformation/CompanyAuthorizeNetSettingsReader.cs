using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Contracts.Clients;

namespace Fgs.Billing.Infrastructure.Entities.CustomerPaymentInformation;

internal sealed class CompanyAuthorizeNetSettingsReader(ISetupClient setupClient) : ICompanyAuthorizeNetSettings
{
    public const string ProviderCode = "AUTHORIZE_NET";

    public async Task<CompanyAuthorizeNetSettingsResult> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await setupClient.GetCompanyProviderCredentialAsync(ProviderCode, cancellationToken);
            if (response is null)
            {
                return CompanyAuthorizeNetSettingsResult.Unavailable();
            }

            if (!response.Success || response.Data is null)
            {
                return response.StatusCode == 404
                    ? CompanyAuthorizeNetSettingsResult.Missing()
                    : CompanyAuthorizeNetSettingsResult.Unavailable();
            }

            var data = response.Data;
            if (string.IsNullOrWhiteSpace(data.ApiLoginId)
                || string.IsNullOrWhiteSpace(data.PublicClientKey)
                || string.IsNullOrWhiteSpace(data.TransactionKey))
            {
                return CompanyAuthorizeNetSettingsResult.Missing();
            }

            return CompanyAuthorizeNetSettingsResult.Found(
                new CompanyAuthorizeNetSettings(
                    data.ApiLoginId.Trim(),
                    data.PublicClientKey.Trim(),
                    data.TransactionKey.Trim(),
                    data.UseSandbox));
        }
        catch (HttpRequestException)
        {
            return CompanyAuthorizeNetSettingsResult.Unavailable();
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return CompanyAuthorizeNetSettingsResult.Unavailable();
        }
    }
}
