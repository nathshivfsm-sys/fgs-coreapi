FROM nginx:stable-alpine

RUN apk add --no-cache curl \
    && mkdir -p /var/cache/nginx/api /var/log/nginx /etc/nginx/certs \
    && rm -f /etc/nginx/conf.d/default.conf

COPY nginx.conf /etc/nginx/nginx.conf
COPY conf.d/site.conf /etc/nginx/conf.d/site.conf
COPY conf.d/includes/ /etc/nginx/conf.d/includes/
COPY proxy_params.conf /etc/nginx/proxy_params.conf
COPY cache_params.conf /etc/nginx/cache_params.conf

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD curl -fsS http://localhost/nginx-health || exit 1

EXPOSE 80 443
