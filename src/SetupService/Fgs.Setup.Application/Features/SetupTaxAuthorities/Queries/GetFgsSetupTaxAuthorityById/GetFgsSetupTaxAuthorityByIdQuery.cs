using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.GetFgsSetupTaxAuthorityById;

public sealed record GetFgsSetupTaxAuthorityByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupTaxAuthorityDetailDto>>;
