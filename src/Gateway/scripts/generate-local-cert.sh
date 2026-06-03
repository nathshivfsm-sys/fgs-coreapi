#!/usr/bin/env sh
set -eu

DAYS="${DAYS:-365}"
ROOT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")/.." && pwd)"
CERT_DIR="$ROOT_DIR/certs"
CONFIG_PATH="$CERT_DIR/localhost-openssl.cnf"

if ! command -v openssl >/dev/null 2>&1; then
  echo "OpenSSL is required." >&2
  exit 1
fi

mkdir -p "$CERT_DIR"

cat > "$CONFIG_PATH" <<'EOF'
[req]
default_bits = 2048
prompt = no
default_md = sha256
distinguished_name = dn
req_extensions = req_ext
x509_extensions = req_ext

[dn]
C = US
ST = Local
L = Local
O = FGS
OU = Local Development
CN = localhost

[req_ext]
subjectAltName = @alt_names

[alt_names]
DNS.1 = localhost
DNS.2 = nginx
IP.1 = 127.0.0.1
IP.2 = ::1
EOF

openssl req -x509 -nodes -days "$DAYS" -newkey rsa:2048 \
  -keyout "$CERT_DIR/localhost.key" \
  -out "$CERT_DIR/localhost.crt" \
  -config "$CONFIG_PATH" \
  -extensions req_ext

rm -f "$CONFIG_PATH"

echo "Created $CERT_DIR/localhost.crt and $CERT_DIR/localhost.key"
