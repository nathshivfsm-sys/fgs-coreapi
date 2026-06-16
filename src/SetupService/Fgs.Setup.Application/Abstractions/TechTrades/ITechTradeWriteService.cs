using Fgs.Setup.Application.Features.TechTrades.Dtos;

namespace Fgs.Setup.Application.Abstractions.TechTrades;

public interface ITechTradeWriteService
{
    Task<TechTradeDetailDto> CreateAsync(TechTradeCreateDto dto, CancellationToken cancellationToken = default);

    Task<TechTradeDetailDto> UpdateAsync(long id, TechTradeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<TechTradeDetailDto> PatchAsync(long id, TechTradePatchDto dto, CancellationToken cancellationToken = default);

    Task<TechTradeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
