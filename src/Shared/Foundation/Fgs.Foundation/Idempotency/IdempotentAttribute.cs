namespace Fgs.Foundation.Idempotency;

/// <summary>
/// Marks an action as eligible for <c>Idempotency-Key</c> replay caching.
/// When the header is present, duplicate requests return the cached response.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class IdempotentAttribute : Attribute;
