using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;

namespace Fgs.Bff.API.GraphQL;

/// <summary>
/// Safe GraphQL diagnostics: operation name, duration, and errors — never logs variables or full queries.
/// </summary>
public sealed class FgsGraphQlDiagnosticObserver(ILogger<FgsGraphQlDiagnosticObserver> logger)
    : ExecutionDiagnosticEventListener
{
    public override IDisposable ExecuteRequest(IRequestContext context)
    {
        var operationName = context.Request.OperationName ?? "anonymous";
        var sw = System.Diagnostics.Stopwatch.StartNew();
        return new RequestScope(logger, operationName, sw);
    }

    public override void ResolverError(IMiddlewareContext context, IError error)
    {
        logger.LogWarning(
            "GraphQL resolver error on {FieldName}: {ErrorCode} {ErrorMessage}",
            context.Selection.Field.Name,
            error.Code,
            error.Message);
    }

    public override void RequestError(IRequestContext context, Exception exception)
    {
        logger.LogError(
            exception,
            "GraphQL request error for operation {OperationName}",
            context.Request.OperationName ?? "anonymous");
    }

    private sealed class RequestScope(
        ILogger logger,
        string operationName,
        System.Diagnostics.Stopwatch sw) : IDisposable
    {
        public void Dispose()
        {
            sw.Stop();
            logger.LogInformation(
                "GraphQL operation {OperationName} completed in {Duration}ms",
                operationName,
                sw.ElapsedMilliseconds);
        }
    }
}
