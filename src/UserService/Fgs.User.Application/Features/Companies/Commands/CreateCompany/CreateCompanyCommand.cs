using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Companies.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Commands.CreateCompany;

public sealed record CreateCompanyCommand(long TenantId, CompanyCreateDto Dto)
    : IRequest<ApiResponse<CompanyDetailDto>>;
