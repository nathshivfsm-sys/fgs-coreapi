using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.DeleteFgsSetupTaxAuthority;

public sealed record DeleteFgsSetupTaxAuthorityCommand(long Id)
    : IRequest<ApiResponse<FgsSetupTaxAuthorityDetailDto>>;
