using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Contracts.Api;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using MediatR;

namespace Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.DeleteCustomerPaymentInformation;

public sealed class DeleteCustomerPaymentInformationCommandHandler(
    ICompanyAuthorizeNetSettings settings,
    IAuthorizeNetCustomerProfileClient authorizeNet,
    ICustomerPaymentInformationReadRepository readRepository,
    ICustomerPaymentInformationWriteService writeService)
    : IRequestHandler<DeleteCustomerPaymentInformationCommand, ApiResponse<CustomerPaymentInformationGridDto>>
{
    public async Task<ApiResponse<CustomerPaymentInformationGridDto>> Handle(
        DeleteCustomerPaymentInformationCommand request,
        CancellationToken cancellationToken)
    {
        var card = await readRepository.GetCardAsync(request.Id, cancellationToken);
        if (card is null)
        {
            return CustomerPaymentInformationResponses.NotFound<CustomerPaymentInformationGridDto>();
        }

        if (!string.IsNullOrWhiteSpace(card.CustomerProfileId)
            && !string.IsNullOrWhiteSpace(card.CustomerPaymentProfileId))
        {
            var lookup = await settings.GetAsync(cancellationToken);
            if (!CustomerPaymentInformationResponses.TryGetSettings<CustomerPaymentInformationGridDto>(
                    lookup,
                    out var merchant,
                    out var failure))
            {
                return failure!;
            }

            var deleted = await authorizeNet.DeleteCustomerPaymentProfileAsync(
                merchant,
                card.CustomerProfileId,
                card.CustomerPaymentProfileId,
                cancellationToken);
            if (!deleted.IsOk && !deleted.IsProfileNotFound)
            {
                return CustomerPaymentInformationResponses.MapProviderFailure<CustomerPaymentInformationGridDto>(deleted)
                    ?? CustomerPaymentInformationResponses.Rejected<CustomerPaymentInformationGridDto>();
            }
        }

        var grid = await writeService.SoftDeleteAsync(request.Id, cancellationToken);
        if (grid is null)
        {
            return CustomerPaymentInformationResponses.NotFound<CustomerPaymentInformationGridDto>();
        }

        return ApiResponse<CustomerPaymentInformationGridDto>.Ok(grid);
    }
}
