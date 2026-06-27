using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.CreateFgsSetupTaxAuthority;

public sealed record CreateFgsSetupTaxAuthorityCommand(FgsSetupTaxAuthorityCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxAuthorityDetailDto>>;
