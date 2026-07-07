# Outbox Pattern

Within the same transaction: 1. Save business data. 2. Save Outbox
event. 3. Commit.

A background publisher sends events to RabbitMQ.
