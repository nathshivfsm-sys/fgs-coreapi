using System.Net;

namespace Fgs.Foundation.Middleware;

public sealed record ExceptionMapping(HttpStatusCode StatusCode, IReadOnlyList<string> Errors);

public interface IExceptionStatusMapper
{
    bool TryMap(Exception exception, out ExceptionMapping mapping);
}
