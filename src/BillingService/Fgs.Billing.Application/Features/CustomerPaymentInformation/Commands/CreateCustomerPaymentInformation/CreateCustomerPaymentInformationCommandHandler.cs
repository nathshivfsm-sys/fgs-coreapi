using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.CreateCustomerPaymentInformation;

public sealed class CreateCustomerPaymentInformationCommandHandler(
    ICompanyAuthorizeNetSettings settings,
    IAuthorizeNetCustomerProfileClient authorizeNet,
    ICustomerPaymentInformationReadRepository readRepository,
    ICustomerPaymentInformationWriteService writeService)
    : IRequestHandler<CreateCustomerPaymentInformationCommand, ApiResponse<CustomerPaymentInformationGridDto>>
{
    public async Task<ApiResponse<CustomerPaymentInformationGridDto>> Handle(
        CreateCustomerPaymentInformationCommand request,
        CancellationToken cancellationToken)
    {
        var lookup = await settings.GetAsync(cancellationToken);
        if (!CustomerPaymentInformationResponses.TryGetSettings<CustomerPaymentInformationGridDto>(
                lookup,
                out var merchant,
                out var failure))
        {
            return failure!;
        }

        var profileId = await ResolveCustomerProfileIdAsync(request.Dto.ArCustomerId, merchant, cancellationToken);
        if (profileId.Failure is not null)
        {
            return profileId.Failure;
        }

        var opaqueData = new AuthorizeNetOpaqueData(
            request.Dto.OpaqueData.DataDescriptor.Trim(),
            request.Dto.OpaqueData.DataValue.Trim());
        var created = await authorizeNet.CreateCustomerPaymentProfileAsync(
            merchant,
            profileId.ProfileId!,
            opaqueData,
            request.Dto.CardHolderName.Trim(),
            request.Dto.BillingZip.Trim(),
            cancellationToken);
        var createdFailure = CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(created);
        if (createdFailure is not null || string.IsNullOrWhiteSpace(created.CustomerPaymentProfileId))
        {
            return createdFailure
                ?? CustomerPaymentInformationResponses.IncompleteCard<CustomerPaymentInformationGridDto>();
        }

        var paymentProfileId = created.CustomerPaymentProfileId;
        var details = await authorizeNet.GetCustomerPaymentProfileAsync(
            merchant,
            profileId.ProfileId!,
            paymentProfileId,
            cancellationToken);
        if (!HasCardDetails(details))
        {
            await CompensateAsync(merchant, profileId.ProfileId!, paymentProfileId, cancellationToken);
            return CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(details)
                ?? CustomerPaymentInformationResponses.IncompleteCard<CustomerPaymentInformationGridDto>();
        }

        try
        {
            var grid = await writeService.CreateAsync(
                new CustomerPaymentInformationCreateModel(
                    request.Dto.ArCustomerId,
                    profileId.ProfileId!,
                    paymentProfileId,
                    request.Dto.CardHolderName.Trim(),
                    details.CardBrand,
                    details.Last4!,
                    details.ExpirationMonth!.Value,
                    details.ExpirationYear!.Value,
                    request.Dto.SetAsDefault),
                cancellationToken);
            return ApiResponse<CustomerPaymentInformationGridDto>.Ok(grid, ApiStatusCodes.Created);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            await CompensateAsync(merchant, profileId.ProfileId!, paymentProfileId, cancellationToken);
            return CustomerPaymentInformationResponses.SaveFailed<CustomerPaymentInformationGridDto>();
        }
    }

    private async Task<(string? ProfileId, ApiResponse<CustomerPaymentInformationGridDto>? Failure)> ResolveCustomerProfileIdAsync(
        long arCustomerId,
        CompanyAuthorizeNetSettings merchant,
        CancellationToken cancellationToken)
    {
        var existing = await readRepository.FindCustomerProfileIdAsync(arCustomerId, cancellationToken);
        if (!string.IsNullOrWhiteSpace(existing))
        {
            return (existing, null);
        }

        var merchantCustomerId = arCustomerId.ToString();
        var created = await authorizeNet.CreateCustomerProfileAsync(merchant, merchantCustomerId, cancellationToken);
        if (created.IsDuplicate)
        {
            var loaded = await authorizeNet.GetCustomerProfileByMerchantCustomerIdAsync(
                merchant,
                merchantCustomerId,
                cancellationToken);
            if (loaded.IsOk && !string.IsNullOrWhiteSpace(loaded.CustomerProfileId))
            {
                return (loaded.CustomerProfileId, null);
            }

            return (null, CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(loaded)
                ?? CustomerPaymentInformationResponses.Rejected<CustomerPaymentInformationGridDto>());
        }

        if (created.IsOk && !string.IsNullOrWhiteSpace(created.CustomerProfileId))
        {
            return (created.CustomerProfileId, null);
        }

        return (null, CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(created)
            ?? CustomerPaymentInformationResponses.Rejected<CustomerPaymentInformationGridDto>());
    }

    private async Task CompensateAsync(
        CompanyAuthorizeNetSettings merchant,
        string customerProfileId,
        string customerPaymentProfileId,
        CancellationToken cancellationToken)
    {
        try
        {
            await authorizeNet.DeleteCustomerPaymentProfileAsync(
                merchant,
                customerProfileId,
                customerPaymentProfileId,
                cancellationToken);
        }
        catch (Exception)
        {
            // The saved card was not stored. A second cleanup attempt is not made.
        }
    }

    private static bool HasCardDetails(AuthorizeNetCallResult details) =>
        details.IsOk
        && details.Last4 is { Length: 4 }
        && details.Last4.All(char.IsDigit)
        && details.ExpirationMonth is >= 1 and <= 12
        && details.ExpirationYear is not null;
}
