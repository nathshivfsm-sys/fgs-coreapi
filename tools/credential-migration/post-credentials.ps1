param(
    [string]$BaseUrl = "http://localhost:5004",
    [switch]$UpdateDatabaseOnly,
    [switch]$UpdateAwsOnly,
    [switch]$UpdateRedisOnly,
    [switch]$RedisOnly
)

$ErrorActionPreference = "Stop"
$endpoint = "$BaseUrl/api/v1/credential"

$platformKmsKeyArn = "arn:aws:kms:us-east-1:286093098927:key/8ad55556-fcb0-4dd7-8ed1-4de526a38a78"
$platformAwsPayloadJson = (@{
    AccessKeyId       = "AKIAUFHD2H6XTIMP765N"
    SecretAccessKey   = "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"
    KmsKeyArn         = $platformKmsKeyArn
    Region            = "us-east-1"
    BucketNamePrefix  = "fgs-dev-tenant"
    ApplicationSlug   = "fsm"
} | ConvertTo-Json -Compress)

function Get-RdsConnectionString {
    param([string]$Database)

    $dbHost = "ls-95525d03d63ded62abc7e8fb350fc1bd8c854c0f.cjuqwweywlnx.us-east-2.rds.amazonaws.com"
    $dbUser = "dbmasteruser"
    $dbPassword = '2eswD$8%rX`S%CAaDf0-LHfyueSKLzr~'
    return "Host=$dbHost;Port=5432;Database=$Database;Username=$dbUser;Password=$dbPassword"
}

$databaseConnections = [ordered]@{
    FgsUser             = "fgs_dev_db"
    FgsUserReadOnly     = "fgs_dev_db"
    FgsSetup            = "fgs_dev_db"
    FgsSetupReadOnly    = "fgs_dev_db"
    FgsFile             = "fgs_dev_db"
    FgsNotification     = "fgs_dev_db"
    FgsConsumer         = "fgs_dev_db"
    FgsAudit            = "fgs_dev_db"
    FgsAsset            = "fgs_dev_db"
    FgsAssetReadOnly    = "fgs_dev_db"
    FgsBilling          = "fgs_dev_db"
    FgsCommunication    = "fgs_dev_db"
    FgsServiceAgreement = "fgs_dev_db"
    FgsCrm              = "fgs_dev_db"
    FgsDispatch         = "fgs_dev_db"
    FgsIntegration      = "fgs_dev_db"
    FgsInventory        = "fgs_dev_db"
    FgsReporting        = "fgs_dev_db"
}

$databasePayload = @{}
foreach ($entry in $databaseConnections.GetEnumerator()) {
    $databasePayload[$entry.Key] = Get-RdsConnectionString -Database $entry.Value
}
$databasePayloadJson = $databasePayload | ConvertTo-Json -Compress

$redisPayloadJson = (@{
    Enabled                          = $true
    ConnectionString                 = "redis:6379"
    InstanceName                     = "fgs:"
    DefaultAbsoluteExpirationMinutes = 30
} | ConvertTo-Json -Compress)

$entraPayloadJson = (@{
    TenantId           = "f9417a96-cb71-4919-8332-7087f1ad0455"
    ClientId           = "3c788340-59a5-4864-b1b4-4f9adeffcb37"
    ClientSecret       = "6O68Q~ryTEIP6I_cHvXzLdh0~gArxSebXxNXMdiw"
    Authority          = "https://fsdemoapp.ciamlogin.com"
    RedirectUri        = "https://localhost:8443/api/v1/auth/entra/callback"
    LoginRedirectUri   = "https://localhost:3000/auth/callback"
    Scopes             = "openid profile email offline_access"
    UserFlow           = "Fgs_SignUpSignIn"
    PasswordUserFlow   = "Fgs_SignUpSignIn_Pwd"
    AuthorizeEndpoint  = ""
    TokenEndpoint      = ""
} | ConvertTo-Json -Compress)

$sendGridPayloadJson = (@{
    ApiKey      = "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"
    FromAddress = "fieldgoodservice@gmail.com"
    FromName    = "FGS Platform"
} | ConvertTo-Json -Compress)

$rabbitMqPayloadJson = (@{
    Username = "fgs"
    Password = "fgsdevlocal"
} | ConvertTo-Json -Compress)

function Get-GlobalCredentials {
    $list = Invoke-RestMethod -Uri "${endpoint}?scope=global&activeOnly=true" -Method Get
    if ($list.data) { return @($list.data) }
    if ($list.Data) { return @($list.Data) }
    return @()
}

function Upsert-Credential {
    param(
        [string]$ProviderCode,
        [string]$CredentialName,
        [string]$PayloadJson
    )

    $existing = Get-GlobalCredentials | Where-Object {
        ($_.providerCode -eq $ProviderCode) -or ($_.ProviderCode -eq $ProviderCode)
    } | Select-Object -First 1

    if ($existing) {
        $id = if ($existing.id) { $existing.id } else { $existing.Id }
        $name = if ($existing.credentialName) { $existing.credentialName } else { $CredentialName }
        Write-Host "PUT $ProviderCode (id=$id)..."
        $body = @{
            credentialName = $name
            payload        = $PayloadJson
            isActive       = $true
        } | ConvertTo-Json -Compress

        $response = Invoke-RestMethod -Uri "${endpoint}/${id}?scope=global" -Method Put -ContentType "application/json" -Body $body
        $ok = $response.success -or $response.Success
        if (-not $ok) {
            $errors = if ($response.errors) { $response.errors } else { $response.Errors }
            throw "PUT $ProviderCode failed: $($errors -join '; ')"
        }
        Write-Host "  Updated OK"
    }
    else {
        Write-Host "POST $ProviderCode..."
        $body = @{
            scope          = "global"
            providerCode   = $ProviderCode
            credentialName = $CredentialName
            payload        = $PayloadJson
        } | ConvertTo-Json -Compress

        $response = Invoke-RestMethod -Uri $endpoint -Method Post -ContentType "application/json" -Body $body
        $ok = $response.success -or $response.Success
        if (-not $ok) {
            $errors = if ($response.errors) { $response.errors } else { $response.Errors }
            throw "POST $ProviderCode failed: $($errors -join '; ')"
        }
        $newId = if ($response.data.id) { $response.data.id } else { $response.Data.Id }
        Write-Host "  Created OK id=$newId"
    }
}

if ($UpdateAwsOnly) {
    Upsert-Credential -ProviderCode "AWS" -CredentialName "platform-aws" -PayloadJson $platformAwsPayloadJson
    Write-Host "AWS credential upserted."
    return
}

if ($UpdateRedisOnly -or $RedisOnly) {
    Upsert-Credential -ProviderCode "REDIS" -CredentialName "platform-redis" -PayloadJson $redisPayloadJson
    Write-Host "REDIS credential upserted."
    return
}

if ($UpdateDatabaseOnly) {
    Upsert-Credential -ProviderCode "DATABASE" -CredentialName "platform-databases" -PayloadJson $databasePayloadJson
    Write-Host "DATABASE credential upserted ($($databaseConnections.Count) keys)."
    return
}

Write-Host "Upserting all platform credentials on $BaseUrl..."
Upsert-Credential -ProviderCode "DATABASE" -CredentialName "platform-databases" -PayloadJson $databasePayloadJson
Upsert-Credential -ProviderCode "RABBITMQ" -CredentialName "platform-rabbitmq" -PayloadJson $rabbitMqPayloadJson
Upsert-Credential -ProviderCode "REDIS" -CredentialName "platform-redis" -PayloadJson $redisPayloadJson
Upsert-Credential -ProviderCode "AWS" -CredentialName "platform-aws" -PayloadJson $platformAwsPayloadJson
Upsert-Credential -ProviderCode "ENTRA_EXTERNAL_ID" -CredentialName "platform-entra" -PayloadJson $entraPayloadJson
Upsert-Credential -ProviderCode "SENDGRID" -CredentialName "platform-sendgrid" -PayloadJson $sendGridPayloadJson
Write-Host "All credentials upserted successfully."
