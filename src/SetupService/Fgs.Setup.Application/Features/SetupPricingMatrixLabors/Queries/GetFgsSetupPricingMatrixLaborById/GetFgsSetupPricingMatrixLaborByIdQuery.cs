using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.GetFgsSetupPricingMatrixLaborById;

public sealed record GetFgsSetupPricingMatrixLaborByIdQuery(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>;
