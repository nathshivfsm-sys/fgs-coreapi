#!/bin/sh
# Serve nginx on :80 only (ALB terminates TLS). Mirrors gateway_start in deployment/aws/terraform/locals.tf
# Writes swagger routes at runtime so older nginx images without swagger-routes.ec2.conf still work.
set -eu

mkdir -p /etc/nginx/conf.d/includes

cat > /etc/nginx/conf.d/includes/upstreams.prod.conf << 'UP'
upstream setup_service {
  least_conn;
  server setup-service:5004 max_fails=3 fail_timeout=10s;
  keepalive 32;
}
upstream user_service { server user-service:5001 max_fails=3 fail_timeout=10s; keepalive 32; }
upstream notification_service { server 127.0.0.1:9; }
upstream bff_service { server bff-service:5003 max_fails=3 fail_timeout=10s; keepalive 32; }
upstream file_service { server 127.0.0.1:9; }
upstream audit_service { server 127.0.0.1:9; }
upstream inventory_service { server 127.0.0.1:9; }
upstream asset_service { server 127.0.0.1:9; }
upstream crm_service { server 127.0.0.1:9; }
upstream scheduling_service { server 127.0.0.1:9; }
upstream billing_service { server 127.0.0.1:9; }
upstream service_agreement_service { server 127.0.0.1:9; }
UP

# Always write EC2 swagger routes so image need not contain this file.
cat > /etc/nginx/conf.d/includes/swagger-routes.ec2.conf << 'SWAGGER'
location = /swagger {
    return 308 /swagger/;
}

location = /swagger/index.html {
    default_type text/html;
    alias /etc/nginx/conf.d/includes/swagger-index.html;
}

location = /swagger/ {
    return 308 /swagger/index.html;
}

location = /swagger/setup {
    return 308 /swagger/setup/;
}

location /swagger/setup/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream setup-service:5004;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}

location = /swagger/user {
    return 308 /swagger/user/;
}

location /swagger/user/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream user-service:5001;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}

location = /swagger/bff {
    return 308 /swagger/bff/;
}

location /swagger/bff/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream bff-service:5003;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}

location = /swagger/audit {
    return 308 /swagger/audit/;
}

location /swagger/audit/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream audit-service:5008;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}

location = /swagger/notification {
    return 308 /swagger/notification/;
}

location /swagger/notification/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream notification-service:5002;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}

location = /swagger/publisher {
    return 308 /swagger/publisher/;
}

location /swagger/publisher/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream publisher-service:5006;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}

location = /swagger/consumer {
    return 308 /swagger/consumer/;
}

location /swagger/consumer/ {
    resolver 127.0.0.11 valid=10s ipv6=off;
    set $swagger_upstream consumer-service:5007;
    proxy_pass http://$swagger_upstream$request_uri;
    include /etc/nginx/proxy_params.conf;
    proxy_cache off;
    proxy_buffering off;
    add_header Cache-Control "no-store" always;
}
SWAGGER

# Minimal index if image has no swagger-index.html
if [ ! -f /etc/nginx/conf.d/includes/swagger-index.html ]; then
  cat > /etc/nginx/conf.d/includes/swagger-index.html << 'IDX'
<!DOCTYPE html>
<html><head><meta charset="utf-8"><title>FGS Swagger</title></head>
<body>
  <h1>FGS API docs</h1>
  <ul>
    <li><a href="/swagger/setup/">Setup</a></li>
    <li><a href="/swagger/user/">User</a></li>
    <li><a href="/swagger/bff/">BFF</a></li>
    <li><a href="/swagger/audit/">Audit</a></li>
    <li><a href="/swagger/notification/">Notification</a></li>
    <li><a href="/swagger/publisher/">Publisher</a></li>
    <li><a href="/swagger/consumer/">Consumer</a></li>
  </ul>
</body></html>
IDX
fi

cat > /etc/nginx/conf.d/site.conf << 'SITE'
include /etc/nginx/conf.d/includes/upstreams.prod.conf;
server {
  listen 80 default_server;
  server_name _;
  location = /nginx-health {
    access_log off;
    return 200 "healthy\n";
    add_header Content-Type text/plain;
  }
  include /etc/nginx/conf.d/includes/api-v1-routes.prod.conf;
  include /etc/nginx/conf.d/includes/swagger-routes.ec2.conf;
}
SITE

exec nginx -g 'daemon off;'
