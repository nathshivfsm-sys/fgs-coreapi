using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.UpdateCustomerPaymentInformation;

public sealed class UpdateCustomerPaymentInformationCommandHandler(
    ICompanyAuthorizeNetSettings settings,
    IAuthorizeNetCustomerProfileClient authorizeNet,
    ICustomerPaymentInformationReadRepository readRepository,
    ICustomerPaymentInformationWriteService writeService)
    : IRequestHandler<UpdateCustomerPaymentInformationCommand, ApiResponse<CustomerPaymentInformationGridDto>>
{
    public async Task<ApiResponse<CustomerPaymentInformationGridDto>> Handle(
        UpdateCustomerPaymentInformationCommand request,
        CancellationToken cancellationToken)
    {
        var card = await readRepository.GetCardAsync(request.Id, cancellationToken);
        if (card is null)
        {
            return CustomerPaymentInformationResponses.NotFound<CustomerPaymentInformationGridDto>();
        }

        if (string.IsNullOrWhiteSpace(card.CustomerProfileId)
            || string.IsNullOrWhiteSpace(card.CustomerPaymentProfileId))
        {
            return ApiResponse<CustomerPaymentInformationGridDto>.Fail(
                ["Saved card is missing payment provider identifiers."],
                ApiStatusCodes.BadRequest);
        }

        var lookup = await settings.GetAsync(cancellationToken);
        if (!CustomerPaymentInformationResponses.TryGetSettings<CustomerPaymentInformationGridDto>(
                lookup,
                out var merchant,
                out var failure))
        {
            return failure!;
        }

        var replacingNumber = request.Dto.OpaqueData is not null;
        if (!replacingNumber && string.IsNullOrWhiteSpace(card.Last4))
        {
            return ApiResponse<CustomerPaymentInformationGridDto>.Fail(
                ["Saved card number is incomplete."],
                ApiStatusCodes.BadRequest);
        }

        var opaqueData = replacingNumber
            ? new AuthorizeNetOpaqueData(
                request.Dto.OpaqueData!.DataDescriptor.Trim(),
                request.Dto.OpaqueData.DataValue.Trim())
            : null;
        var updated = await authorizeNet.UpdateCustomerPaymentProfileAsync(
            merchant,
            card.CustomerProfileId,
            card.CustomerPaymentProfileId,
            request.Dto.CardHolderName.Trim(),
            request.Dto.ExpirationMonth,
            request.Dto.ExpirationYear,
            card.Last4,
            opaqueData,
            cancellationToken);
        var updatedFailure = CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(updated);
        if (updatedFailure is not null)
        {
            return updatedFailure;
        }

        string? cardBrand = null;
        string? last4 = null;
        var expirationMonth = request.Dto.ExpirationMonth;
        var expirationYear = request.Dto.ExpirationYear;
        if (replacingNumber)
        {
            var details = await authorizeNet.GetCustomerPaymentProfileAsync(
                merchant,
                card.CustomerProfileId,
                card.CustomerPaymentProfileId,
                cancellationToken);
            if (!details.IsOk
                || details.Last4 is not { Length: 4 } refreshedLast4
                || !refreshedLast4.All(char.IsDigit)
                || details.ExpirationMonth is not short refreshedMonth
                || refreshedMonth is < 1 or > 12
                || details.ExpirationYear is not short refreshedYear)
            {
                return CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(details)
                    ?? CustomerPaymentInformationResponses.IncompleteCard<CustomerPaymentInformationGridDto>();
            }

            cardBrand = details.CardBrand;
            last4 = refreshedLast4;
            expirationMonth = refreshedMonth;
            expirationYear = refreshedYear;
        }

        var grid = await writeService.UpdateAsync(
            request.Id,
            new CustomerPaymentInformationUpdateModel(
                request.Dto.CardHolderName.Trim(),
                cardBrand,
                last4,
                expirationMonth,
                expirationYear,
                request.Dto.IsDefault),
            cancellationToken);
        if (grid is null)
        {
            return CustomerPaymentInformationResponses.NotFound<CustomerPaymentInformationGridDto>();
        }

        return ApiResponse<CustomerPaymentInformationGridDto>.Ok(grid);
    }
}
