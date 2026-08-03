using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.GetFgsSetupPricingMatrixOtherById;

public sealed record GetFgsSetupPricingMatrixOtherByIdQuery(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>;
