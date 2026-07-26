$ErrorActionPreference = "Stop"
$key = "fgs-internal-credential-distribution-key"
$headers = @{ "X-Fgs-Internal-Service-Key" = $key }
$r = Invoke-RestMethod -Uri "http://setup-service:5004/api/v1/credential/resolved?serviceName=fgs-user-service" -Headers $headers
$vals = $r.data.values
$interesting = @($vals.PSObject.Properties.Name | Where-Object { $_ -match "ENTRA|DATABASE:FgsUser|REDIS|AWS:|PasswordUserFlow|SENDGRID" } | Sort-Object)
Write-Host "Matched keys ($($interesting.Count)):"
$interesting | ForEach-Object { Write-Host "  $_" }
Write-Host "PasswordUserFlow=$($vals.'Global:ENTRA_EXTERNAL_ID:PasswordUserFlow')"
Write-Host "UserFlow=$($vals.'Global:ENTRA_EXTERNAL_ID:UserFlow')"
Write-Host "ClientId set=$([bool]$vals.'Global:ENTRA_EXTERNAL_ID:ClientId')"
Write-Host "FgsUser CS set=$([bool]$vals.'Global:DATABASE:FgsUser')"
Write-Host "Redis CS set=$([bool]$vals.'Global:REDIS:ConnectionString')"
Write-Host "AWS AccessKey set=$([bool]$vals.'Global:AWS:AccessKeyId')"
