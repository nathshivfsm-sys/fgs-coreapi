using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.LookupSetupTaxAuthorities;

public sealed record LookupSetupTaxAuthoritiesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>>;
