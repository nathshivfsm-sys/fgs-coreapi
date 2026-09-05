namespace Fgs.Messaging.Models;

/// <summary>
/// Transport-agnostic publish target.
/// RabbitMQ: <see cref="DestinationName"/> = exchange, <see cref="RoutingKey"/> = routing key.
/// Future SQS/SNS: map destination to queue URL or topic ARN.
/// </summary>
public sealed record IntegrationEventDestination(string DestinationName, string RoutingKey);
