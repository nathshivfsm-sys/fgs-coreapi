using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Companies.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Commands.UpdateCompany;

public sealed record UpdateCompanyCommand(long TenantId, long CompanyId, CompanyUpdateDto Dto)
    : IRequest<ApiResponse<CompanyDetailDto>>;
