# Push EC2 compose/deploy files to /opt/fgs via SSM (no SSH, no paste).
# Requires: AWS CLI v2 + Session Manager plugin, IAM permission for ssm:SendCommand.
#
# Usage:
#   .\deployment\aws\ec2\Push-Ec2Files.ps1
#   .\deployment\aws\ec2\Push-Ec2Files.ps1 -InstanceId i-0123456789abcdef0

param(
    [string]$InstanceId = "i-007e873bef51e4b17",
    [string]$Region = "us-east-1",
    [string]$RemoteDir = "/opt/fgs"
)

$ErrorActionPreference = "Stop"
$Root = Join-Path $PSScriptRoot "."
$Files = @(
    @{ Local = "docker-compose.ec2.yml"; Remote = "docker-compose.ec2.yml"; Mode = "0644" },
    @{ Local = "deploy-service.sh"; Remote = "deploy-service.sh"; Mode = "0755" },
    @{ Local = "nginx-http-only-entrypoint.sh"; Remote = "nginx-http-only-entrypoint.sh"; Mode = "0755" }
)

function Wait-SsmCommand([string]$CommandId) {
    for ($i = 0; $i -lt 36; $i++) {
        Start-Sleep -Seconds 2
        $inv = aws ssm get-command-invocation `
            --command-id $CommandId `
            --instance-id $InstanceId `
            --region $Region `
            --output json | ConvertFrom-Json
        if ($inv.Status -in @("Success", "Cancelled", "TimedOut", "Failed")) {
            return $inv
        }
    }
    throw "Timed out waiting for SSM command $CommandId"
}

Write-Host "Ensuring $RemoteDir exists on $InstanceId..."
$mkdirId = aws ssm send-command `
    --instance-ids $InstanceId `
    --document-name "AWS-RunShellScript" `
    --parameters "commands=[`"sudo mkdir -p $RemoteDir`"]" `
    --region $Region `
    --query "Command.CommandId" `
    --output text
$mkdirResult = Wait-SsmCommand $mkdirId
if ($mkdirResult.Status -ne "Success") {
    throw "mkdir failed: $($mkdirResult.StandardErrorContent)"
}

foreach ($f in $Files) {
    $localPath = Join-Path $Root $f.Local
    if (-not (Test-Path $localPath)) {
        throw "Missing file: $localPath"
    }

    $remotePath = "$RemoteDir/$($f.Remote)"
    $b64 = [Convert]::ToBase64String([IO.File]::ReadAllBytes($localPath))
    Write-Host "Uploading $($f.Local) -> $remotePath"

    # Write base64 to a temp file on the instance via stdin chunking is awkward in SSM;
    # send as a single shell script that decodes base64.
    $script = @"
set -euo pipefail
echo '$b64' | base64 -d | sudo tee '$remotePath' > /dev/null
sudo chmod $($f.Mode) '$remotePath'
ls -la '$remotePath'
"@

    $tmp = [IO.Path]::GetTempFileName()
    try {
        # AWS CLI expects JSON for parameters; write a parameters file to avoid escaping hell.
        $paramsObj = @{ commands = @($script) }
        $paramsJson = $paramsObj | ConvertTo-Json -Compress
        Set-Content -Path $tmp -Value $paramsJson -Encoding utf8NoBOM

        $cmdId = aws ssm send-command `
            --instance-ids $InstanceId `
            --document-name "AWS-RunShellScript" `
            --parameters "file://$tmp" `
            --region $Region `
            --query "Command.CommandId" `
            --output text

        $result = Wait-SsmCommand $cmdId
        if ($result.Status -ne "Success") {
            Write-Host $result.StandardOutputContent
            Write-Host $result.StandardErrorContent
            throw "Upload failed for $($f.Local): $($result.Status)"
        }
        Write-Host "  OK: $($result.StandardOutputContent.Trim())"
    }
    finally {
        Remove-Item $tmp -ErrorAction SilentlyContinue
    }
}

Write-Host ""
Write-Host "Done. On EC2 you can deploy with:"
Write-Host "  sudo $RemoteDir/deploy-service.sh audit-service dev"
Write-Host "  sudo $RemoteDir/deploy-service.sh notification-service dev"
Write-Host "  sudo $RemoteDir/deploy-service.sh publisher-service dev"
Write-Host "  sudo $RemoteDir/deploy-service.sh consumer-service dev"
Write-Host "  sudo $RemoteDir/deploy-service.sh nginx dev"
