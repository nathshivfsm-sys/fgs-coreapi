namespace Fgs.Security.Abstractions;

public interface IFgsUserStatusValidator
{
    Task<bool> IsActiveAsync(CancellationToken cancellationToken = default);
}
