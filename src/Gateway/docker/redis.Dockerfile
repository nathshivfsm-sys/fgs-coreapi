# Pinned Redis for FGS EC2. Build from repo root:
#   docker build -f src/Gateway/docker/redis.Dockerfile -t fgs-redis:dev .
ARG SERVICE_VERSION=7-alpine
FROM public.ecr.aws/docker/library/redis:${SERVICE_VERSION}

HEALTHCHECK --interval=15s --timeout=5s --start-period=10s --retries=6 \
    CMD redis-cli ping | grep -q PONG
