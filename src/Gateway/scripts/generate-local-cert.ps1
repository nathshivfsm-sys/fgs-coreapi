param(
    [int]$Days = 365
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$certDir = Join-Path $root "certs"
New-Item -ItemType Directory -Force $certDir | Out-Null

$configPath = Join-Path $certDir "localhost-openssl.cnf"
$keyPath = Join-Path $certDir "localhost.key"
$certPath = Join-Path $certDir "localhost.crt"

if (-not (Get-Command openssl -ErrorAction SilentlyContinue)) {
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        throw "OpenSSL or the .NET SDK is required to generate local PEM certificates."
    }

    $temp = Join-Path ([System.IO.Path]::GetTempPath()) "fgs-nginx-certgen"
    dotnet new console --force -o $temp | Out-Null

    @'
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

var certDir = args[0];
var days = int.Parse(args[1]);
Directory.CreateDirectory(certDir);

using var rsa = RSA.Create(2048);
var request = new CertificateRequest(
    "CN=developer.fsm.com, O=FGS, OU=Local Development, L=Local, S=Local, C=US",
    rsa,
    HashAlgorithmName.SHA256,
    RSASignaturePadding.Pkcs1);

var san = new SubjectAlternativeNameBuilder();
san.AddDnsName("developer.fsm.com");
san.AddDnsName("localhost");
san.AddDnsName("nginx");
san.AddIpAddress(IPAddress.Parse("127.0.0.1"));
san.AddIpAddress(IPAddress.IPv6Loopback);
request.CertificateExtensions.Add(san.Build());
request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
request.CertificateExtensions.Add(new X509KeyUsageExtension(
    X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment,
    true));
request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") },
    false));
request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

using var cert = request.CreateSelfSigned(
    DateTimeOffset.UtcNow.AddDays(-1),
    DateTimeOffset.UtcNow.AddDays(days));

File.WriteAllText(
    Path.Combine(certDir, "localhost.crt"),
    PemEncoding.WriteString("CERTIFICATE", cert.RawData),
    Encoding.ASCII);
File.WriteAllText(
    Path.Combine(certDir, "localhost.key"),
    PemEncoding.WriteString("PRIVATE KEY", rsa.ExportPkcs8PrivateKey()),
    Encoding.ASCII);
'@ | Set-Content -Path (Join-Path $temp "Program.cs") -Encoding utf8

    dotnet run --project $temp -- $certDir $Days
    Write-Host "Created $certPath and $keyPath"
    return
}

@"
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
CN = developer.fsm.com

[req_ext]
subjectAltName = @alt_names

[alt_names]
DNS.1 = developer.fsm.com
DNS.2 = localhost
DNS.3 = nginx
IP.1 = 127.0.0.1
IP.2 = ::1
"@ | Set-Content -Path $configPath -Encoding ascii

openssl req -x509 -nodes -days $Days -newkey rsa:2048 `
    -keyout $keyPath `
    -out $certPath `
    -config $configPath `
    -extensions req_ext

Remove-Item $configPath -Force

Write-Host "Created $certPath and $keyPath"
