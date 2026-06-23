param(
    [string]$BaseUrl = "http://localhost:5071",
    [switch]$UpdateDatabaseOnly,
    [switch]$UpdateAwsOnly
)

$platformKmsKeyArn = "arn:aws:kms:us-east-1:286093098927:key/8ad55556-fcb0-4dd7-8ed1-4de526a38a78"
$platformAwsPayloadJson = (@{
    AccessKeyId     = "AKIAUFHD2H6XTIMP765N"
    SecretAccessKey = "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"
    KmsKeyArn       = $platformKmsKeyArn
} | ConvertTo-Json -Compress)

$ErrorActionPreference = "Stop"
$endpoint = "$BaseUrl/api/v1/credentials"

function Get-RdsConnectionString {
    param([string]$Database)

    $dbHost = "ls-95525d03d63ded62abc7e8fb350fc1bd8c854c0f.cjuqwweywlnx.us-east-2.rds.amazonaws.com"
    $dbUser = "dbmasteruser"
    $dbPassword = '2eswD$8%rX`S%CAaDf0-LHfyueSKLzr~'
    return "Host=$dbHost;Port=5432;Database=$Database;Username=$dbUser;Password=$dbPassword"
}

# All platform services share the RDS dev database.
$databaseConnections = [ordered]@{
    FgsUser             = "fgs_dev_db"
    FgsUserReadOnly     = "fgs_dev_db"
    FgsSetup            = "fgs_dev_db"
    FgsSetupReadOnly    = "fgs_dev_db"
    FgsFile             = "fgs_dev_db"
    FgsNotification   = "fgs_dev_db"
    FgsConsumer       = "fgs_dev_db"
    FgsAudit          = "fgs_dev_db"
    FgsAsset          = "fgs_dev_db"
    FgsBilling        = "fgs_dev_db"
    FgsCommunication  = "fgs_dev_db"
    FgsServiceAgreement = "fgs_dev_db"
    FgsCrm            = "fgs_dev_db"
    FgsDispatch       = "fgs_dev_db"
    FgsIntegration    = "fgs_dev_db"
    FgsInventory      = "fgs_dev_db"
    FgsReporting      = "fgs_dev_db"
}

$databasePayload = @{}
foreach ($entry in $databaseConnections.GetEnumerator()) {
    $databasePayload[$entry.Key] = Get-RdsConnectionString -Database $entry.Value
}
$databasePayloadJson = $databasePayload | ConvertTo-Json -Compress

if ($UpdateAwsOnly) {
    Write-Host "Updating AWS credential on $BaseUrl..."
    $list = Invoke-RestMethod -Uri "${endpoint}?scope=global&activeOnly=true" -Method Get
    $awsCred = $list.data | Where-Object { $_.providerCode -eq "AWS" } | Select-Object -First 1
    if (-not $awsCred) {
        throw "AWS credential not found. Run post-credentials.ps1 without -UpdateAwsOnly first."
    }

    $body = @{
        credentialName = $awsCred.credentialName
        payload        = $platformAwsPayloadJson
    } | ConvertTo-Json -Compress

    $response = Invoke-RestMethod -Uri "${endpoint}/$($awsCred.id)?scope=global" -Method Put -ContentType "application/json" -Body $body
    if (-not $response.success) {
        throw "Failed: $($response.errors -join '; ')"
    }

    Write-Host "  OK ($($response.statusCode)) id=$($awsCred.id)"
    Write-Host "AWS credential updated (AccessKeyId, SecretAccessKey, KmsKeyArn for consumer services)."
    return
}

if ($UpdateDatabaseOnly) {
    Write-Host "Updating DATABASE credential on $BaseUrl..."
    $list = Invoke-RestMethod -Uri "${endpoint}?scope=global&activeOnly=true" -Method Get
    $dbCred = $list.data | Where-Object { $_.providerCode -eq "DATABASE" } | Select-Object -First 1
    if (-not $dbCred) {
        throw "DATABASE credential not found. Run post-credentials.ps1 without -UpdateDatabaseOnly first."
    }

    $body = @{
        credentialName = $dbCred.credentialName
        payload        = $databasePayloadJson
    } | ConvertTo-Json -Compress

    $response = Invoke-RestMethod -Uri "${endpoint}/$($dbCred.id)?scope=global" -Method Put -ContentType "application/json" -Body $body
    if (-not $response.success) {
        throw "Failed: $($response.errors -join '; ')"
    }

    Write-Host "  OK ($($response.statusCode)) id=$($dbCred.id)"
    Write-Host "DATABASE credential updated for $($databaseConnections.Count) connection keys (all RDS)."
    return
}

$payloads = @(
    @{
        scope          = "global"
        providerCode   = "DATABASE"
        credentialName = "platform-databases"
        payload        = $databasePayloadJson
    },
    @{
        scope          = "global"
        providerCode   = "RABBITMQ"
        credentialName = "platform-rabbitmq"
        payload        = '{"Username":"fgs","Password":"fgsdevlocal"}'
    },
    @{
        scope          = "global"
        providerCode   = "AWS"
        credentialName = "platform-aws"
        payload        = $platformAwsPayloadJson
    },
    @{
        scope          = "global"
        providerCode   = "ENTRA_EXTERNAL_ID"
        credentialName = "platform-entra"
        payload        = '{"TenantId":"f9417a96-cb71-4919-8332-7087f1ad0455","ClientId":"3c788340-59a5-4864-b1b4-4f9adeffcb37","ClientSecret":"6O68Q~ryTEIP6I_cHvXzLdh0~gArxSebXxNXMdiw","Authority":"https://fsdemoapp.ciamlogin.com","RedirectUri":"https://localhost:8443/api/v1/auth/entra/callback","Scopes":"openid profile email offline_access","UserFlow":"Fgs_SignUpSignIn","AuthorizeEndpoint":"","TokenEndpoint":""}'
    },
    @{
        scope          = "global"
        providerCode   = "SENDGRID"
        credentialName = "platform-sendgrid"
        payload        = '{"ApiKey":"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","FromAddress":"fieldgoodservice@gmail.com","FromName":"FGS Platform"}'
    }
)

foreach ($body in $payloads) {
    Write-Host "POST $($body.providerCode)..."
    $json = $body | ConvertTo-Json -Compress
    try {
        $response = Invoke-RestMethod -Uri $endpoint -Method Post -ContentType "application/json" -Body $json
        if (-not $response.success) {
            throw "Failed: $($response.errors -join '; ')"
        }
        Write-Host "  OK ($($response.statusCode)) id=$($response.data.id)"
    }
    catch {
        $status = $_.Exception.Response.StatusCode.value__
        $reader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
        $detail = $reader.ReadToEnd()
        throw "POST $($body.providerCode) failed ($status): $detail"
    }
}

Write-Host "All credentials posted successfully."
