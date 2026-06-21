using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.LookupSetupPaymentTerms;

public sealed record LookupSetupPaymentTermsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>>;
