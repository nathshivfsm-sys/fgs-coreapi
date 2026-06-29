using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.LookupSetupPaymentMethods;

public sealed record LookupSetupPaymentMethodsQuery(
    bool ActiveOnly = true,
    bool? IsMobileVisible = null,
    bool? IsCustomerPortalVisible = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupPaymentMethodLookupDto>>>;
