#!/bin/sh
# Serve nginx on :80 only (ALB terminates TLS). Mirrors gateway_start in deployment/aws/terraform/locals.tf
set -eu

cat > /etc/nginx/conf.d/includes/upstreams.prod.conf << 'UP'
upstream setup_service {
  least_conn;
  server setup-service:5004 max_fails=3 fail_timeout=10s;
  keepalive 32;
}
upstream user_service { server user-service:5001 max_fails=3 fail_timeout=10s; keepalive 32; }
upstream notification_service { server 127.0.0.1:9; }
upstream bff_service { server 127.0.0.1:9; }
upstream file_service { server 127.0.0.1:9; }
upstream audit_service { server 127.0.0.1:9; }
upstream inventory_service { server 127.0.0.1:9; }
upstream asset_service { server 127.0.0.1:9; }
upstream crm_service { server 127.0.0.1:9; }
upstream scheduling_service { server 127.0.0.1:9; }
upstream billing_service { server 127.0.0.1:9; }
upstream service_agreement_service { server 127.0.0.1:9; }
UP

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
