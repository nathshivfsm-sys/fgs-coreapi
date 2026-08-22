# Pinned RabbitMQ (management) for FGS EC2. Build from repo root:
#   docker build -f src/Gateway/docker/rabbitmq.Dockerfile -t fgs-rabbitmq:dev .
ARG SERVICE_VERSION=4-management-alpine
FROM public.ecr.aws/docker/library/rabbitmq:${SERVICE_VERSION}

HEALTHCHECK --interval=15s --timeout=10s --start-period=40s --retries=6 \
    CMD rabbitmq-diagnostics -q ping
