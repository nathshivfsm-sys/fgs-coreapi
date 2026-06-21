using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.GetTitleOfCourtesyById;

public sealed class GetTitleOfCourtesyByIdQueryHandler(ITitleOfCourtesyReadRepository readRepository)
    : IRequestHandler<GetTitleOfCourtesyByIdQuery, ApiResponse<TitleOfCourtesyDetailDto>>
{
    public async Task<ApiResponse<TitleOfCourtesyDetailDto>> Handle(
        GetTitleOfCourtesyByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<TitleOfCourtesyDetailDto>.Fail(
                    [$"Title of courtesy '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<TitleOfCourtesyDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<TitleOfCourtesyDetailDto>(ex);
        }
    }
}
