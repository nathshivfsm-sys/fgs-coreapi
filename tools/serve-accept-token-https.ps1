# Serves accept-token.html over HTTPS on https://localhost:5500
# Requires: PowerShell + Python 3 (or py launcher)
# Usage:  powershell -ExecutionPolicy Bypass -File .\serve-accept-token-https.ps1

$ErrorActionPreference = "Stop"
$here = $PSScriptRoot
Set-Location $here

$html = Join-Path $here "accept-token.html"
if (-not (Test-Path $html)) {
    throw "accept-token.html not found next to this script: $here"
}

$certPem = Join-Path $here "accept-token-localhost.pem"
$keyPem = Join-Path $here "accept-token-localhost-key.pem"
$pfx = Join-Path $here "accept-token-localhost.pfx"
$pwdText = "accept-token-local"
$securePwd = ConvertTo-SecureString -String $pwdText -Force -AsPlainText

if (-not (Test-Path $certPem) -or -not (Test-Path $keyPem)) {
    Write-Host "Creating self-signed localhost certificate..."
    $cert = New-SelfSignedCertificate `
        -DnsName "localhost" `
        -CertStoreLocation "Cert:\CurrentUser\My" `
        -NotAfter (Get-Date).AddYears(2) `
        -KeyExportPolicy Exportable `
        -KeySpec KeyExchange

    Export-PfxCertificate -Cert $cert -FilePath $pfx -Password $securePwd | Out-Null

    $pythonExport = @"
from pathlib import Path
try:
    from cryptography.hazmat.primitives.serialization import Encoding, PrivateFormat, NoEncryption, pkcs12
except ImportError as ex:
    raise SystemExit(
        "Python package 'cryptography' is required once to export the cert.`n"
        "Run:  py -m pip install cryptography`n"
        f"Detail: {ex}"
    )

pfx_path = Path(r"$pfx")
cert_path = Path(r"$certPem")
key_path = Path(r"$keyPem")
data = pfx_path.read_bytes()
key, cert, _ = pkcs12.load_key_and_certificates(data, b"$pwdText")
cert_path.write_bytes(cert.public_bytes(Encoding.PEM))
key_path.write_bytes(key.private_bytes(Encoding.PEM, PrivateFormat.TraditionalOpenSSL, NoEncryption()))
print("Wrote", cert_path.name, "and", key_path.name)
"@

    $py = Get-Command py -ErrorAction SilentlyContinue
    $python = Get-Command python -ErrorAction SilentlyContinue
    if ($py) {
        $pythonExport | & py -3 -
    } elseif ($python) {
        $pythonExport | & python -
    } else {
        throw "Python not found. Install Python 3, or open accept-token.html through any existing HTTPS host."
    }
}

$servePy = @"
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
import ssl
from pathlib import Path

here = Path(r"$here")
cert = here / "accept-token-localhost.pem"
key = here / "accept-token-localhost-key.pem"

class Handler(SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=str(here), **kwargs)

httpd = ThreadingHTTPServer(("127.0.0.1", 5500), Handler)
ctx = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
ctx.load_cert_chain(certfile=str(cert), keyfile=str(key))
httpd.socket = ctx.wrap_socket(httpd.socket, server_side=True)
print("Serving https://localhost:5500/accept-token.html")
print("Accept the browser certificate warning once, then tokenize.")
httpd.serve_forever()
"@

Write-Host "Starting HTTPS server..."
$py = Get-Command py -ErrorAction SilentlyContinue
if ($py) {
    $servePy | & py -3 -
} else {
    $servePy | & python -
}
